import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GlobalsService {
constructor() { }
public baseUrl = 'http://localhost:5000/';
public displayAdvertStyle = 'table';
public pageSize = 10;
}
