import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GlobalsService } from '../global/globals.service';
import { Observable } from 'rxjs';
import { Category } from 'src/app/Models/Category';

@Injectable({
  providedIn: 'root'
})
export class CatService {

baseUrl: string;

constructor(private http: HttpClient, private config: GlobalsService) {
  this.baseUrl = this.config.baseUrl + 'api/categories';
}

getAll(): Observable<any> {
  return this.http.get(`${this.baseUrl}`);
}

getAllWithOptions(options: any): Observable<any> {
  const params = new HttpParams({fromObject: options});
  return this.http.get(`${this.baseUrl}?${params}`);
}


getById(categoryId): Observable<any> {
  return this.http.get(`${this.baseUrl}/${categoryId}`);
}

create(cat: Category): Observable<any> {
  return this.http.post(this.baseUrl, cat);
}
delete(cat: Category): Observable<any> {
  return this.http.delete(this.baseUrl + '/' + cat.categoryId);
}

update(cat: Category): Observable<any> {
  return this.http.put(this.baseUrl, cat);
}

}
