import { Component, Input } from '@angular/core';
import { Advert } from 'src/app/Models/Advert';
import {environment} from 'src/environments/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-adtiles',
  templateUrl: './adtiles.component.html',
  styleUrls: ['./adtiles.component.css']
})
export class AdtilesComponent {
  baseUrl: string;
  @Input() data: Advert[];
  constructor(private router: Router) {
    this.baseUrl = environment.baseUrl;
  }

  onCardClicked(id: number) {
    this.router.navigate(['ads', 'details', `${id}`]);
  }
}
