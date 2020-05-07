import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Advert } from 'src/app/Models/Advert';
import { BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-adtablebody',
  templateUrl: './adtablebody.component.html',
  styleUrls: ['./adtablebody.component.css']
})
export class AdtablebodyComponent implements OnInit {
  @Input() advertisements: Advert[];
  @Input() displayedColumns: string[];
  @Output() sort: EventEmitter<any> = new EventEmitter<any>();
  @Output() delete: EventEmitter<any> = new EventEmitter<any>();
  // ['title', 'description', 'price', 'category', 'town', 'date', 'manage']
  constructor(breakpointObserver: BreakpointObserver) {
    breakpointObserver.observe(['(max-width: 600px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
        ['title', 'price', 'date'] :
        ['title', 'price', 'category', 'date', ];
    });

    breakpointObserver.observe(['(max-width: 1300px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['title', 'price', 'category',  'date', 'manage'] :
        ['title',  'price', 'category', 'town', 'date', 'manage'];
    });

    breakpointObserver.observe(['(max-width: 1156px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['title',  'price', 'category',  'date',] :
        ['title',  'price', 'category', 'town', 'date'];
    });

    breakpointObserver.observe(['(max-width: 900px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['title', 'price', 'date'] :
        ['title',  'price', 'category',  'date'];
    });

    breakpointObserver.observe(['(max-width: 700px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['title',  'price', 'date'] :
      ['title',  'price', 'category',  'date'];
    });
  }

  ngOnInit() {
  }

  sortData($event) {
    this.sort.emit($event);
  }

  removeAd(id) {
    this.delete.emit(id);
  }

}
