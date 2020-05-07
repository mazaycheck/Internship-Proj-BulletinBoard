import { Component, OnInit } from '@angular/core';
import { BrandService } from 'src/app/services/Repositories/brand.service';
import { ToastrService } from 'ngx-toastr';
import { BrandCategoryService } from 'src/app/services/Repositories/brandCategory.service';
import { CatService } from 'src/app/services/Repositories/cat.service';
import { Brand } from 'src/app/Models/brand';
import { BrandCategory } from 'src/app/Models/BrandCategory';
import { Observable, merge, empty, concat } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Category } from 'src/app/Models/Category';
import { PageEvent } from '@angular/material/paginator';
import { FormControl } from '@angular/forms';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { BrandListUpdateComponent } from './brand-list-update/brand-list-update.component';
import { CheckboxUpdateModalComponent } from '../checkbox-update-modal/checkbox-update-modal.component';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-brandList',
  templateUrl: './brandList.component.html',
  styleUrls: ['./brandList.component.css']
})
export class BrandListComponent implements OnInit {


  allBrandsFromDb: Brand[] = [];
  allCategoriesFromDb: string[] = [];
  displayedColumns = ['title', 'categories', 'action'];

  filter = new FormControl('');

  newEntity: string;

  // Page data
  queryOptions: { pageNumber: number, pageSize: number, query?: string, category?: string } = { pageNumber: 1, pageSize: 10 };
  totalBrandsEntriesInDb: number;
  currentPageNumber: number;
  currentPageSize: number;
  pageSizeOptions = [10, 25, 50];

  constructor(private toastr: ToastrService, private brandsService: BrandService, private categoriesService: CatService,
              private dialog: MatDialog) { }

  ngOnInit() {
    this.getAllBrands();
    this.getAllCategories();
    this.filter.valueChanges.pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(x => { this.queryOptions.query = x; this.getAllBrands(); });
  }

  getAllCategories() {
    this.categoriesService.getAll().subscribe(response => {
      this.allCategoriesFromDb = response.map(x => x.title);
    });
  }

  getAllBrands() {
    this.brandsService.getAll(this.queryOptions).subscribe(response => {
      this.allBrandsFromDb = response.pageData;
      this.totalBrandsEntriesInDb = response.totalEntries;
      this.currentPageNumber = response.pageNumber;
      this.currentPageSize = response.currentPageSize;

      if (this.allBrandsFromDb.length === 0) {
        this.toastr.warning('No data');
      }
    });
  }



  // allBrandsEditOff() {
  //   this.allBrandsFromDb.forEach(element => {
  //     element.edit = false;
  //   });

  // }

  // editSave(brand) {
  //   this.brandsService.updateCategories(brand.brandId, this.temporaryBrandTitle, this.temporaryCategories).subscribe(response => {
  //     this.toastr.success(`${brand.title} was updated`);
  //     brand.categories = [...this.temporaryCategories].sort();
  //     brand.title = this.temporaryBrandTitle;
  //     this.temporaryBrandTitle = '';
  //     brand.edit = false;
  //   },
  //     error => {
  //       this.toastr.error(error);
  //       brand.edit = false;
  //     });

  // }

  // editCancel(entity) {
  //   this.allBrandsEditOff();
  // }

  update(brand) {
    // this.allBrandsEditOff();
    // this.temporaryCategories = [...brand.categories];
    // this.temporaryBrandTitle = brand.title;
    this.onUpdateClicked(brand);
  }

  pageClicked($event: PageEvent) {
    this.queryOptions.pageNumber = $event.pageIndex + 1;
    this.queryOptions.pageSize = $event.pageSize;
    // this.globals.pageSize = $event.pageSize;
    this.getAllBrands();
  }

  createNewEntity() {
    this.brandsService.create(this.newEntity).subscribe(response => {
      this.toastr.success('Created a new brand: ' + this.newEntity);
      this.resetNewEntity();
    });
  }

  delete(brand) {
    // const index = this.allBrandsFromDb.indexOf(brand);
    // this.allBrandsFromDb.splice(index, 1);
    // this.allBrandsFromDb = [...this.allBrandsFromDb];
    this.brandsService.delete(brand).subscribe(response => {
      this.toastr.warning('Deleteted brand: ' + brand.title);
      this.getAllBrands();
    });
  }

  resetNewEntity() {
    this.newEntity = '';
  }



  // onUpdateClicked(data: any) {
  //   const config = new MatDialogConfig();
  //   config.minWidth = '380px';
  //   config.width = '600px';
  //   config.data = data;
  //   console.log(config);
  //   this.dialog.open(BrandListUpdateComponent, config);
  // }


  onUpdateClicked(brand: Brand) {
    const dataToInject = {
      identity: brand.brandId,
      title: brand.title,
      service: this.brandsService,
      entity: brand,
      allSelectOptions: this.allCategoriesFromDb,
      currentSelection: brand.categories,
      editTitle: true
    };
    const config = new MatDialogConfig();
    config.minWidth = '380px';
    config.width = '600px';
    config.data = dataToInject;
    this.dialog.open(CheckboxUpdateModalComponent, config);
    this.dialog.afterAllClosed.subscribe(x => {
      this.getAllBrands();
    });
  }


}
