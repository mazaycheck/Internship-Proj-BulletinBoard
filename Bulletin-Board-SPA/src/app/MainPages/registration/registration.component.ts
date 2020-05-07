import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { FormsModule, NgForm, NgModel } from '@angular/forms';
import { UserRegisterModel } from '../../Models/UserRegisterModel';
import { Observable } from 'rxjs';
import { NgForOf } from '@angular/common';
import { Router } from '@angular/router';
import { ErrorInterceptorService } from '../../services/err/errorInterceptor.service';
import { ToastrService } from 'ngx-toastr';
import {MatDialog, MatDialogConfig, MatDialogRef} from '@angular/material/dialog';
import { Town } from 'src/app/Models/Town';
import { TownService } from 'src/app/services/Repositories/town.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(private service: AuthService, private router: Router, private toast: ToastrService, private dialog: MatDialog,
              private townService: TownService) { }

 model: UserRegisterModel = { userName: '', password: '', email: '', townId: 0, phoneNumber: '' };
 towns: Town[] = [];
 selectedTownId: number;
 checkout: boolean;


  registerForm: FormsModule;

  ngOnInit() {
    this.townService.getAll().subscribe(response => {
      this.towns = response;
    });
  }

  selectedTownChanged($event) {
    console.log($event);
    console.log($event.value);
    console.log(this.model);
    this.selectedTownId = $event.value;
  }

  redirectoToHome() {
    this.router.navigateByUrl('/ads');
  }

  register() {
    console.log(this.model);
    this.service.register(this.model).subscribe(
      response => {
        this.toast.success('Registered!');
        this.dialog.closeAll();
        if (response) {
          if (this.checkout === true) {
          this.service.login(this.model).subscribe(r => {
            this.service.settoken(r.token);
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
