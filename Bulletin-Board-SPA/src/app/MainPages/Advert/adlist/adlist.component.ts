import { Component} from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';


@Component({
  selector: 'app-adlist',
  templateUrl: './adlist.component.html',
  styleUrls: ['./adlist.component.css']
})
export class AdlistComponent {

  displayedColumns: string[] = ['Title', 'Price', 'Category', 'Town', 'CreateDate'];
  titleLength = 50;

  constructor(breakpointObserver: BreakpointObserver) {

    breakpointObserver.observe(['(max-width: 600px)']).subscribe(result => {
      this.titleLength = result.matches ? 45 : 50;
      this.displayedColumns = result.matches ?
      ['Title'] :
        ['Title', 'Price'];
    });

    breakpointObserver.observe(['(max-width: 800px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['Title', 'Price'] :
        ['Title',  'Price', 'Category','CreateDate'];
    });

    breakpointObserver.observe(['(max-width: 1300px)']).subscribe(result => {
      this.displayedColumns = result.matches ?
      ['Title', 'Price', 'Category'] :
        ['Title',  'Price', 'Category', 'Town', 'CreateDate'];
    });
  }
}
