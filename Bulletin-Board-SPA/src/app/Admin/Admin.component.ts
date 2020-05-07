import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/Repositories/user.service';
import { UserForDetail } from '../Models/UserForDetail';
import { TownService } from '../services/Repositories/town.service';
import { CatService } from '../services/Repositories/cat.service';
import { MatTabChangeEvent } from '@angular/material/tabs';

@Component({
  selector: 'app-Admin',
  templateUrl: './Admin.component.html',
  styleUrls: ['./Admin.component.css']
})
export class AdminComponent implements OnInit {

  users: UserForDetail[] = [];
  constructor(public userService: UserService, public townService: TownService, public catService: CatService) { }

  ngOnInit() {

  }

  tabchanged($event: MatTabChangeEvent) {

  }

}
