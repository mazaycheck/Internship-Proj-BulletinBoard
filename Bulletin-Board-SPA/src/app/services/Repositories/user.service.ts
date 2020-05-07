import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserForDetail } from 'src/app/Models/UserForDetail';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = environment.baseUrl + 'api/users/';
  }

  getAll(options: any): Observable<any> {
    console.log(options);
    const params = new HttpParams({fromObject: options});
    return this.http.get(`${this.baseUrl}?${params}`);
  }
  
  getById(id: number): Observable <UserForDetail> {
    return this.http.get<UserForDetail>(this.baseUrl + `${id}`);
   }

}
