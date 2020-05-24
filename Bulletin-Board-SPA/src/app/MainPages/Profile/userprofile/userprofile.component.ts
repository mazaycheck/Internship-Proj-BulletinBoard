import { Component, OnInit, Input } from '@angular/core';
import { UserForDetail } from 'src/app/Models/UserForDetail';
import { Advert } from 'src/app/Models/Advert';
import { ActivatedRoute } from '@angular/router';
import { AdvertService } from 'src/app/services/Repositories/advert.service';
import { UserService } from 'src/app/services/Repositories/user.service';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent implements OnInit {

  userId: number;
  userInfo: UserForDetail;
  adverts: Advert[] = [];
  columns: string[] = ['Title', 'Price', 'Category', 'CreateDate'];
  titleLength = 35;

  placeholderImageUrl = '/assets/images/profile.png';
  constructor(private route: ActivatedRoute, private userservice: UserService,
    private advertService: AdvertService, private authService: AuthService,
    breakpointObserver: BreakpointObserver) {

    breakpointObserver.observe(['(max-width: 600px)']).subscribe(result => {

      this.columns = result.matches ?
        ['Title'] :
        ['Title', 'Price'];

      if (this.canManage()) {
        this.columns = result.matches ?
          ['Title', 'Manage'] :
          ['Title', 'Manage'];
        this.titleLength = result.matches ? 25 : 30;
      }

    });

    breakpointObserver.observe(['(max-width: 800px)']).subscribe(result => {
      this.columns = result.matches ?
        ['Title', 'Price'] :
        ['Title', 'Price', 'Category'];
      if (this.canManage()) {
        this.columns = result.matches ?
          ['Title',  'Manage'] :
          ['Title',  'Manage'];
        this.titleLength = result.matches ? 25 : 30;
      }
    });

    breakpointObserver.observe(['(max-width: 1300px)']).subscribe(result => {
      this.columns = result.matches ?
        ['Title', 'Price', 'Category'] :
        ['Title', 'Price', 'Category', 'CreateDate'];
      if (this.canManage()) {
        this.columns = result.matches ?
          ['Title', 'Manage'] :
          ['Title', 'Price', 'CreateDate', 'Manage'];

        this.titleLength = result.matches ? 30 : 35;
      }
    });

  }
  ngOnInit() {
    this.userId = +this.route.snapshot.paramMap.get('id');
    this.loadUserInfo();
    if (this.canManage()) {
      this.columns = ['Title', 'Price', 'CreateDate', 'Manage'];
    };
  }


  canManage(): boolean {
    return this.getCurrentUserId() == this.userId;
  }

  getCurrentUserId(): number {
    const userId = this.authService.gettCurrentUserId();
    return userId;
  }

  loadUserInfo() {
    this.userservice.getById(this.userId).subscribe(response => {
      this.userInfo = response;
    });
  }
}
