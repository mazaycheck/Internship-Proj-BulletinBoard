import { Component} from '@angular/core';


@Component({
  selector: 'app-adlist',
  templateUrl: './adlist.component.html',
  styleUrls: ['./adlist.component.css']
})
export class AdlistComponent {

  columns: string[] = ['Title', 'Price', 'Category', 'Town', 'CreateDate', 'Manage'];

}
