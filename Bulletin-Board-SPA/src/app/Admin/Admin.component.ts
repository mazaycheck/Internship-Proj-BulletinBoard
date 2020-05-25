import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/Data/user.service';
import { UserForDetail } from '../Models/UserForDetail';
import { TownService } from '../services/Data/town.service';
import { CatService } from '../services/Data/cat.service';
import { MatTabChangeEvent } from '@angular/material/tabs';

@Component({
  selector: 'app-Admin',
  templateUrl: './Admin.component.html',
  styleUrls: ['./Admin.component.css']
})
export class AdminComponent  {
  users: UserForDetail[] = [];
  constructor(public userService: UserService, public townService: TownService, public catService: CatService) { }
}
