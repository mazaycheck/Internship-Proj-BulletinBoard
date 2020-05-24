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
  @Input() titleLength: number;
  @Output() sort: EventEmitter<any> = new EventEmitter<any>();
  @Output() delete: EventEmitter<any> = new EventEmitter<any>();

  constructor(breakpointObserver: BreakpointObserver) {
   
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
