import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserRegisterModel } from 'src/app/Models/UserRegisterModel';
import { GlobalsService } from '../global/globals.service';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl: string;
  constructor(private http: HttpClient, private globals: GlobalsService) {
    this.baseUrl = globals.baseUrl + 'api/auth/';
  }

  login(model): Observable<any> {
    return this.http.post(this.baseUrl + 'login', model);
  }

  logout() {
    localStorage.removeItem('token');
  }
  register(model): Observable<any> {
    return this.http.post(this.baseUrl + 'register', model);
  }

  settoken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(): string {
    return localStorage.getItem('token');
  }

  isLoggedIn() {
    const token = this.getToken();
    if (token) {
      const decoded = jwt_decode(token);
      if (decoded.exp > (Date.now().valueOf() / 1000)) {
        return true;
      } else {
        localStorage.removeItem('token');
        return false;
      }
    } else {
      return false;
    }
  }

  getCurrentUserName() {
    const token = this.getToken();
    if (token) {
      const decoded = jwt_decode(token);
      return decoded.unique_name;
    }
  }

  gettCurrentUserId() {
    const token = this.getToken();
    if (token) {
      const decoded = jwt_decode(token);
      return decoded.nameid;
    }
  }
  getRoles(): any {
    const token = this.getToken();
    if (token) {
      const decoded = jwt_decode(token);
      return decoded.role;
    }
  }

  hasRequiredRoles(requeredRoles: string[]) {
    const userRoles = this.getRoles();
    if (userRoles instanceof Array) {
      return (userRoles as string[]).some(v => requeredRoles.indexOf(v));
    } else {
      return requeredRoles.includes(userRoles as string);
    }
  }
}
