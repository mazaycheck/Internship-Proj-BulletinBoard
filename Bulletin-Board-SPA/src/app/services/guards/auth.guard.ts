import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  canActivate(): boolean  {
    if (this.auth.isLoggedIn()) {
      return true;
    }
    this.toast.error('Must be logged in!');
    this.router.navigateByUrl('/');
    return false;
  }

  constructor(private router: Router, private toast: ToastrService, private auth: AuthService){

  }
}
