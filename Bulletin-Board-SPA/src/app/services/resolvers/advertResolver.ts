import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Advert } from 'src/app/Models/Advert';
import { AdvertService } from '../Repositories/advert.service';

@Injectable({
  providedIn: 'root'
})
export class AdvertResolver implements Resolve<Advert> {

constructor(private advertService: AdvertService) { }
  resolve(route: import("@angular/router").ActivatedRouteSnapshot, state: import("@angular/router").RouterStateSnapshot): Advert | import("rxjs").Observable<Advert> | Promise<Advert> {
    return this.advertService.getAd(+route.paramMap.get('id'));
  }

}
