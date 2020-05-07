import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import {MatDialog, MatDialogConfig, MatDialogRef} from '@angular/material/dialog';
import { RegistrationComponent } from '../registration/registration.component';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  // encapsulation : ViewEncapsulation.None,
})
export class NavComponent implements OnInit {
  
  hide = true;
  loginData: any = {};
  name: string;

  constructor(public authService: AuthService, private toast: ToastrService, private router: Router, private dialog: MatDialog) { }

  ngOnInit() {
    if (this.isLoggedIn()) {
      this.name = this.authService.getCurrentUserName();
    }
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
    this.dialog.open(RegistrationComponent, config);
  }

}
