import { Component, OnInit, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import {MatDialog, MatDialogConfig, MatDialogRef} from '@angular/material/dialog';
import { RegistrationComponent } from '../registration/registration.component';
import { MessagesService } from 'src/app/services/Repositories/messages.service';
import * as signalR from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { EventEmitterService } from 'src/app/services/Repositories/event-emitter.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {

  hide = true;
  loginData: any = {};
  name: string;
  userId: number;
  newMessages = 0;

  hubconnection: signalR.HubConnection;
  baseUrl = environment.baseUrl;
  userToken: string;

  constructor(public authService: AuthService, private toast: ToastrService,
              private router: Router, private dialog: MatDialog, private messageService: MessagesService,
              private eventEmitterService: EventEmitterService) { }
  

  ngOnInit() {
    if (this.isLoggedIn()) {
      this.afterLoggedIn();
      this.subscribeToMessageEvent();
    }
  }

  afterLoggedIn() {
    this.name = this.authService.getCurrentUserName();
    this.userId = this.authService.gettCurrentUserId();
    this.userToken = this.authService.getToken();
    this.startConnection();
    this.addDataListening();
    this.updateMessages();
  }

  subscribeToMessageEvent() {
    console.log("test");
    if (this.eventEmitterService.subVar == undefined) {
      this.eventEmitterService.subVar = this.eventEmitterService.invokeFirstComponentFunction
      .subscribe(event => {this.updateMessages(); console.log("EVENT") } );
    }
  }

  startConnection() {
    this.hubconnection = new signalR.HubConnectionBuilder().withUrl(this.baseUrl + 'chatHub',
      { accessTokenFactory: () => this.userToken })
      .build();
    this.hubconnection.start().then(() => console.log('signalR conenction started!')).catch((err) => console.log(err));
  }

  addDataListening() {
    this.hubconnection.on('ReceiveMessage', (signalrdata) => {
      this.updateMessages();
    });
  }

  updateMessages() {
    this.messageService.getUnreadMessages().subscribe(response => {
      this.newMessages = response.length;
    });
  }


  hasRequiredAccess(roles: string[]) {
    return this.authService.hasRequiredRoles(roles);
  }

  login() {

    this.authService.login(this.loginData).subscribe(
      response => {
        if (response) {
          localStorage.setItem('token' , response.token);
          this.name = this.authService.getCurrentUserName();
          this.toast.success('Logged in');
          this.router.navigateByUrl('/ads');
          this.afterLoggedIn();
          
        }
      }, error => {
        this.toast.error(error);
      }
    );
  }

  logout() {
    this.authService.logout();
    this.router.navigateByUrl('/');
    this.toast.warning('Logged out');
  }
  isLoggedIn() {
     return  this.authService.isLoggedIn();
  }

  onRegister() {
    const config = new MatDialogConfig();
    config.width = '30%';
    config.minWidth = '460px';
    this.dialog.open(RegistrationComponent, config);
  }

}
