import { Component } from '@angular/core';
import { Advert } from './Models/Advert';
import { AdvertService } from './services/Repositories/advert.service';
import { ActivatedRoute, Router, RouterEvent, NavigationStart, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'webspa';
  loadingStatus = false;
  constructor(private router: Router) {
    this.configureRouterEventsNavigation();
  }


  private configureRouterEventsNavigation() {
    this.router.events.subscribe((routerEvent: RouterEvent) => {
      if (routerEvent instanceof NavigationStart) {
        this.loadingStatus = true;
      } else if (routerEvent instanceof NavigationEnd) {
        this.loadingStatus = false;
      }
    });
  }
}
