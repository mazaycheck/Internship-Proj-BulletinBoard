import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class JwtTokenInterceptorService implements HttpInterceptor {

  constructor(private authService: AuthService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.authService.isLoggedIn()) {
      const token = localStorage.getItem('token');
      req = req.clone({
        setHeaders : {
          Authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(req);
  }
}
