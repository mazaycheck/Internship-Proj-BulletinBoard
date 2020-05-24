import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AdvertService } from 'src/app/services/Repositories/advert.service';
import { Router } from '@angular/router';
import { CatService } from 'src/app/services/Repositories/cat.service';
import { Category } from 'src/app/Models/Category';
import { ToastrService } from 'ngx-toastr';
import { Town } from 'src/app/Models/Town';
import { TownService } from 'src/app/services/Repositories/town.service';
import { Brand } from 'src/app/Models/brand';
import { BrandService } from 'src/app/services/Repositories/brand.service';
import { BrandCategoryService } from 'src/app/services/Repositories/brandCategory.service';
import { BrandCategory } from 'src/app/Models/BrandCategory';

@Component({
  selector: 'app-adcreate',
  templateUrl: './adcreate.component.html',
  styleUrls: ['./adcreate.component.css']
})
export class AdcreateComponent implements OnInit {
  advertForm: FormGroup;
  allCategoriesFromDb: Category[] = [];
  selectedCategory: Category;
  selectedBrand: string;
  numberOfFiles = 6;
  pictureURLs = new Array<string>(this.numberOfFiles);
  uploadedPicturesBinaryData: string[] = [];
  formData: FormData;
  brandCategoryIdForCreate: number;
  placeholderImageUrl = '/assets/images/camera.jpg';

  constructor(private advertService: AdvertService, private router: Router, private categoriesService: CatService,
              private toast: ToastrService, private brandCatService: BrandCategoryService) { }

  ngOnInit() {
    this.initForm();
    this.getCategories();
    this.initPlaceHolderImages();
    this.formData = new FormData();
  }

  private initPlaceHolderImages() {
    for (let i = 0; i < this.numberOfFiles; i++) {
      this.pictureURLs[i] = this.placeholderImageUrl;
    }
  }

  showErrors($event){
    console.log($event);
  }

  private initForm() {
    this.advertForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      description: new FormControl('', [Validators.maxLength(1000)]),
      price: new FormControl(0, [Validators.min(1), Validators.max(9999999), Validators.required]),
      brand: new FormControl('', [Validators.required]),
      category: new FormControl('', [Validators.required]),
      photo: new FormControl(),
    });
  }

  getCategories() {
    this.categoriesService.getAll().subscribe(
      response => {
        this.allCategoriesFromDb = response;
      }, error => {
        this.toast.error(error);
      }
    );
  }

  getBrandCategoryId(categoryTitle: string, brandTitle: string) {
    let brandCatId;
    this.brandCatService.getBrandCatId(categoryTitle, brandTitle).subscribe(response => {
      brandCatId = response[0].brandCategoryId;
      this.formData.set('brandCategoryId', brandCatId);
      this.brandCategoryIdForCreate = brandCatId;
    });
  }

  getCategory(categoryId: number) {
    this.categoriesService.getById(categoryId).subscribe(response => {
      this.selectedCategory = response;
    }
    );
  }

  onFileAttach($event) {
    this.resetImages();
    for (let i = 0; i < $event.target.files.length; i++) {
      const file = $event.target.files[i];
      this.uploadedPicturesBinaryData.push(file);
      this.formData.append('photo', file, file.name);
      const fileReader = new FileReader();
      fileReader.onload = ((event: any) => {
        this.pictureURLs[i] = event.target.result;
      });
      fileReader.readAsDataURL(file);
    }
  }

  replacePlaceHolderImagesWithRealImages(images: string[]) {
    for (let i = 0; i < images.length; i++) {
      this.pictureURLs[i] = images[i];
    }
  }

  resetImages() {
    this.initPlaceHolderImages();
    this.uploadedPicturesBinaryData = [];
    this.formData.set('photo', null);
  }

  onFileCancel(imageIndex) {
    console.log(imageIndex);
    this.uploadedPicturesBinaryData.splice(imageIndex, 1);
    this.pictureURLs.splice(imageIndex, 1);
    this.pictureURLs.push(this.placeholderImageUrl);
    const photos = this.formData.getAll('photo');
    photos.splice(imageIndex, 1);
    this.formData.set('photo', null);
    this.uploadedPicturesBinaryData.forEach(element => {
      this.formData.append('photo', element, 'textfilename');
    });
  }


  selectedCategoryChanged($event) {
    const categoryId = $event.value;
    if (categoryId > 0) {
      this.getCategory(categoryId);
    }
  }

  selectedBrandChanged($event) {
    this.selectedBrand = $event.value;
    const brand = $event.value;
    this.brandCatService.getBrandCatId(this.selectedCategory.title, this.selectedBrand).subscribe(response => {
      this.brandCategoryIdForCreate = response[0].brandCategoryId;
    });
  }

  toFormData() {
    this.formData.set('title', this.advertForm.value.title);
    this.formData.set('description', this.advertForm.value.description);
    this.formData.set('price', this.advertForm.value.price);
    this.formData.set('brandCategoryId', `${this.brandCategoryIdForCreate}`);
  }

  submit() {
    this.toFormData();
    this.advertService.createAd(this.formData).subscribe(response => {
      this.router.navigateByUrl('/ads');
    }, error => {
      this.toast.error(error);
    }
    );
  }
}
