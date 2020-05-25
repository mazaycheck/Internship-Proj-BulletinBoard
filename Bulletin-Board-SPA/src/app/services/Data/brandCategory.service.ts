import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GlobalsService } from '../global/globals.service';
import { Observable } from 'rxjs';
import { BrandCategory } from 'src/app/Models/BrandCategory';

@Injectable({
  providedIn: 'root'
})
export class BrandCategoryService {

  baseUrl: string;
  constructor(private http: HttpClient, private config: GlobalsService) {
    this.baseUrl = this.config.baseUrl + 'api/brandCategory';
  }

  getBrandCatId(category: string, brand: string): Observable<any> {
    const httpParam = new HttpParams({ fromObject: { category, brand } });
    return this.http.get(`${this.baseUrl}?${httpParam}`);
  }

  getById(id: number): Observable<BrandCategory> {
    return this.http.get<BrandCategory>(this.baseUrl + '/' + `${id}`);
  }

  create(brand: string, category: string): Observable<any> {
    return this.http.post(this.baseUrl, { brand, category });
  }

  delete(brandCategory: BrandCategory): Observable<any> {
    return this.http.delete(this.baseUrl + '/' + brandCategory.brandCategoryId);
  }

  update(brandCategory: BrandCategory): Observable<any> {
    return this.http.put(this.baseUrl, brandCategory);
  }
}
