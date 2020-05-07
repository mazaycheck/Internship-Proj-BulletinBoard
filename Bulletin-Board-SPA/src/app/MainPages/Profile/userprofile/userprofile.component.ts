import { Component, OnInit, Input } from '@angular/core';
import { UserForDetail } from 'src/app/Models/UserForDetail';
import { Advert } from 'src/app/Models/Advert';
import { ActivatedRoute } from '@angular/router';
import { AdvertService } from 'src/app/services/Repositories/advert.service';
import { UserService } from 'src/app/services/Repositories/user.service';

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent implements OnInit {

  userId: number;
  userInfo: UserForDetail;
  adverts: Advert[] = [];
  columns: string[] = ['title', 'price', 'category',  'date'];
  constructor(private route: ActivatedRoute, private userservice: UserService, private advertService: AdvertService) { }
  ngOnInit() {
    this.userId = +this.route.snapshot.paramMap.get('id');
    this.loadUserInfo();
  }

  loadUserInfo() {
    this.userservice.getById(this.userId).subscribe(response => {
      this.userInfo = response;
    });
  }
  loadAdvertsOfUser() {

  }
}
