import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GlobalsService } from '../global/globals.service';
import { Town } from 'src/app/Models/Town';

@Injectable({
  providedIn: 'root'
})
export class TownService {
  baseUrl: string;
  constructor(private http: HttpClient, private config: GlobalsService) {
    this.baseUrl = this.config.baseUrl + 'api/towns';
  }


  getAll(): Observable<any> {
    return this.http.get(this.baseUrl);
  }
  getById() { }
  create(town: Town): Observable<any> {
    return this.http.post(this.baseUrl, town);
  }
  delete(town: Town): Observable<any> {
    return this.http.delete(this.baseUrl + '/' + town.townId);
  }

  update(town: Town): Observable<any> {
    return this.http.put(this.baseUrl, town);
  }
}
