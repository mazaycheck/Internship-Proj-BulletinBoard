import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

baseUrl: string;

constructor(private http: HttpClient) {
  this.baseUrl = environment.baseUrl;
 }

 getRoles(): Observable<any> {
   return this.http.get(this.baseUrl + 'api/roles/roleslist');
 }

 update(email: string, title: string, newRoles: string[]): Observable<any> {
   return this.http.post(this.baseUrl + 'api/roles/editroles', {email, newRoles});
 }

}
