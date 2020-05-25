import { Component, OnInit, OnChanges, SimpleChanges, Input, ViewChild } from '@angular/core';
import { Advert } from '../../../Models/Advert';
import { AdvertService } from '../../../services/Data/advert.service';
import { ToastrService } from 'ngx-toastr';
import { Category } from 'src/app/Models/Category';
import { CatService } from 'src/app/services/Data/cat.service';
import { faSearch, faTrash, faEdit, faList, faTh } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { GlobalsService } from 'src/app/services/global/globals.service';
import { AdvertQueryOptions } from 'src/app/Models/AdvertQueryOptions';
import { PageObject } from 'src/app/Models/PageObject';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { FormControl, FormGroup } from '@angular/forms';
import { debounce, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-adtable',
  templateUrl: './adtable.component.html',
  styleUrls: ['./adtable.component.css']
})
export class AdtableComponent implements OnInit, OnChanges {
  @Input() displayedColumns: string[] = ['Title', 'Price', 'Category', 'Town', 'CreateDate'];
  @Input() userId = 0;
  @Input() titleLength: number;
  @ViewChild('paginator') paginator: MatPaginator;
  advertisements: Advert[] = [];
  categories: Category[] = [];
  queryOptions: AdvertQueryOptions;
  presentationMode: string;
  basePhotoUrl = 'http://localhost:5000/images/';
  pageObject: PageObject;
  pageEvent: PageEvent;
  length = 1000;
  pageSize = 10;
  pageSizeOptions: number[] = [10, 25, 50];

  filter: FormControl;
  advertSourse: Observable<any>;

  private _optionSelected: any;
  public get optionSelected() { return this._optionSelected; }
  public set optionSelected(newValue) {
    this.queryOptions.category = newValue;
    this._optionSelected = newValue;
    this.paginator.pageIndex = 0;
    this.queryOptions.pageNumber = 1;
    this.refresh();
  }

  constructor(private adservice: AdvertService, private catservice: CatService, private toast: ToastrService, private router: Router,
              private globals: GlobalsService) {

    this.queryOptions = new AdvertQueryOptions();
  }


  ngOnChanges(changes: SimpleChanges): void {

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
      .subscribe(x => { this.queryOptions.query = x; this.refresh(); });
  }

  ngOnInit() {
    this.queryOptions.userId = this.userId;
    this.filter = new FormControl('');
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
    this.queryOptions.category = $event.target.value;
    this.queryOptions.pageNumber = 1;
    this.refresh();
  }

  refresh() {
    this.adservice.getAds(this.queryOptions).subscribe(
      response => {
        if (!response) {
          this.toast.warning('No content!');
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

    this.adservice.deleteAd(id).subscribe(response => {
      this.toast.warning(`Removed advert with id ${id}`);
      this.refresh();
    }, error => {
      this.toast.error(error);
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
