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

  constructor(breakpointObserver: BreakpointObserver) {
    breakpointObserver.observe(['(max-width: 600px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
        ['Title', 'Price', 'CreateDate'] :
        ['Title', 'Price', 'Category', 'CreateDate', ];
    });

    breakpointObserver.observe(['(max-width: 1300px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['Title', 'Price', 'Category',  'CreateDate', 'Manage'] :
        ['Title',  'Price', 'Category', 'Town', 'CreateDate', 'Manage'];
    });

    breakpointObserver.observe(['(max-width: 1156px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['Title',  'Price', 'Category',  'CreateDate'] :
        ['Title',  'Price', 'Category', 'Town', 'CreateDate'];
    });

    breakpointObserver.observe(['(max-width: 900px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['Title', 'Price', 'CreateDate'] :
        ['Title',  'Price', 'Category',  'CreateDate'];
    });

    breakpointObserver.observe(['(max-width: 700px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['Title',  'Price', 'CreateDate'] :
      ['Title',  'Price', 'Category',  'CreateDate'];
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
