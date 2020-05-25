import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { FormsModule } from '@angular/forms';
import { UserRegisterModel } from '../../Models/UserRegisterModel';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {MatDialog} from '@angular/material/dialog';
import { Town } from 'src/app/Models/Town';
import { TownService } from 'src/app/services/Data/town.service';
import { EventEmitterService } from 'src/app/services/Data/event-emitter.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(private service: AuthService, private router: Router, private toast: ToastrService, private dialog: MatDialog,
              private townService: TownService, private eventEmitter: EventEmitterService, private authService: AuthService) { }

 model: UserRegisterModel = { userName: '', password: '', email: '', townId: 0, phoneNumber: '' };
 towns: Town[] = [];
 selectedTownId: number;
 checkout: boolean;


  registerForm: FormsModule;

  ngOnInit() {
    this.townService.getAll().subscribe(response => {
      this.towns = response;
    });

    if (this.authService.isLoggedIn()) {
      this.redirectoToHome();
    }
  }

  selectedTownChanged($event) {
    this.selectedTownId = $event.value;
  }

  redirectoToHome() {
    this.router.navigateByUrl('/ads');
  }

  register() {
    this.service.register(this.model).subscribe(
      response => {
        this.toast.success('Registered!');
        this.dialog.closeAll();
        if (response) {
          if (this.checkout === true) {
          this.service.login(this.model).subscribe(r => {
            this.service.settoken(r.token);
            this.eventEmitter.onLoggedInEvent();
             });
          }
          setTimeout(() => {
          this.redirectoToHome();
         }, 1000);
        }
      }, error => {
        this.toast.error(error);
      }
      );
    }
  }
