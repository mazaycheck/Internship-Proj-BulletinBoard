import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { BrandService } from 'src/app/services/Repositories/brand.service';
import { Brand } from 'src/app/Models/brand';
import { ToastrService } from 'ngx-toastr';
import { Category } from 'src/app/Models/Category';

@Component({
  selector: 'app-brand-list-update',
  templateUrl: './brand-list-update.component.html',
  styleUrls: ['./brand-list-update.component.css']
})
export class BrandListUpdateComponent implements OnInit {

  brand: Brand;
  temporaryCategories: string[] = [];
  temporaryBrandTitle: string;
  allCategories: Category[];

  constructor(@Inject(MAT_DIALOG_DATA) public injectedData, private brandService: BrandService, private toastr: ToastrService,
              private dialog: MatDialog) { }

  ngOnInit() {
    this.brand = this.injectedData.brand;
    this.allCategories = this.injectedData.categories;
    this.temporaryCategories = [...this.brand.categories];
    this.temporaryBrandTitle = this.brand.title;
  }
  editSave(brand) {
    this.brandService.update(brand.brandId, this.temporaryBrandTitle, this.temporaryCategories)
      .subscribe(response => {
        this.toastr.success(`${brand.title} was updated`);
        brand.categories = [...this.temporaryCategories].sort();
        brand.title = this.temporaryBrandTitle;
        // this.temporaryBrandTitle = '';
        // brand.edit = false;
        this.dialog.closeAll();
      },
        error => {
          this.toastr.error(error);
          // brand.edit = false;
        });

  }

  editCancel(entity) {
    this.dialog.closeAll();
  }

}
