import { Injectable } from '@angular/core';
import { CanActivateChild, CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

constructor(private router: Router, private toast: ToastrService, private auth: AuthService) { }
  canActivate(): boolean  {
    if (this.auth.isLoggedIn() && this.auth.hasRequiredRoles(['Admin','Moderator'])) {
      return true;
    } else {
      this.toast.error('Must be admin or moderator');
      this.router.navigateByUrl('/');
      return false;
    }
    
  }
}
