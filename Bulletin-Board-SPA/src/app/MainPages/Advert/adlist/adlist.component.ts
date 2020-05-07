import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Advert } from '../../../Models/Advert';
import { AdvertService } from '../../../services/Repositories/advert.service';
import { ToastrService } from 'ngx-toastr';
import { Category } from 'src/app/Models/Category';
import { CatService } from 'src/app/services/Repositories/cat.service';
import { faSearch, faTrash, faEdit, faList, faTh } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { GlobalsService } from 'src/app/services/global/globals.service';
import { AdvertQueryOptions } from 'src/app/Models/AdvertQueryOptions';
import { PageObject } from 'src/app/Models/PageObject';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { FormControl, FormGroup } from '@angular/forms';
import { debounce, debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-adlist',
  templateUrl: './adlist.component.html',
  styleUrls: ['./adlist.component.css']
})
export class AdlistComponent implements OnInit, OnChanges {

  advertisements: Advert[] = [];
  categories: Category[] = [];
  queryOptions: AdvertQueryOptions;
  presentationMode: string;
  basePhotoUrl = 'http://localhost:5000/images/';
  pageObject: PageObject;
  columns: string[] = ['title', 'price', 'category', 'town', 'date', 'manage'];
  pageEvent: PageEvent;
  length = 1000;
  pageSize = 10;
  pageSizeOptions: number[] = [10, 25, 100];

  filter: FormControl;


  private _optionSelected: any;
  public get optionSelected() { return this._optionSelected; }
  public set optionSelected(newValue) {
    this.queryOptions.category = newValue;
    this._optionSelected = newValue;
    this.refresh();
  }

  constructor(private adservice: AdvertService, private catservice: CatService, private toast: ToastrService, private router: Router,
    private globals: GlobalsService) {
    this.queryOptions = new AdvertQueryOptions();
  }


  ngOnChanges(changes: SimpleChanges): void {
    // tslint:disable-next-line: forin
    if (changes.optionSelected) {
      this.queryOptions.category = this.optionSelected;
      this.refresh();
    }
  }

  onOptionSelected(option: string) {
    return option === this.queryOptions?.category;
  }

  switchPresentationMode(mode: string) {
    this.globals.displayAdvertStyle = mode;
    this.presentationMode = mode;
  }

  subscribeToSearchField() {
    this.filter.valueChanges.pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(x => {this.queryOptions.query = x; this.refresh(); } );
  }

  ngOnInit() {
    this.filter =  new FormControl('');
    this.subscribeToSearchField();
    this.presentationMode = this.globals.displayAdvertStyle;
    this.refresh();
    this.catservice.getAll().subscribe(
      response => {
        this.categories = response;
      }, error => {
        this.toast.error(error);
      }
    );
  }

  queryChanged($event) {
    this.refresh();
  }

  goToDetails(id: number) {
    this.router.navigateByUrl(`/ads/details/${id}`);
  }
  selectChanged($event) {
    console.log($event);
    this.queryOptions.category = $event.target.value;
    this.refresh();
  }
  refresh() {
    this.adservice.getAds(this.queryOptions).subscribe(
      response => {
        if (!response) {
          this.toast.warning('No data!');
          this.advertisements = [];
          return;
        }
        this.pageObject = response;
        this.advertisements = this.pageObject.pageData;
        this.length = this.pageObject.pageSize * this.pageObject.totalPages;
      }, error => {
        this.toast.error(error);
      }
    );
  }

  removeAd(id: number) {
    console.log('Delete');
    this.adservice.deleteAd(id).subscribe(response => {
      this.toast.warning(`Removed advert with id ${id}`);
      this.refresh();
    }, error => {
      this.toast.error(`Could not delete advert`);
    }
    );
  }


  pageClicked($event: PageEvent) {    
    this.queryOptions.pageNumber = $event.pageIndex + 1;
    this.queryOptions.pageSize = $event.pageSize;
    this.globals.pageSize = $event.pageSize;
    this.refresh();
  }

  sortData(sort: Sort) {    
    this.queryOptions.orderBy = sort.active;
    this.queryOptions.direction = sort.direction;
    this.refresh();
    
  }

}
