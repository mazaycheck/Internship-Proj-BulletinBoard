import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AdvertService } from 'src/app/services/Repositories/advert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CatService } from 'src/app/services/Repositories/cat.service';
import { Category } from 'src/app/Models/Category';
import { ToastrService } from 'ngx-toastr';
import { Advert } from 'src/app/Models/advert';
import { environment } from 'src/environments/environment';
import { BrandCategory } from 'src/app/Models/BrandCategory';
import { BrandCategoryService } from 'src/app/services/Repositories/brandCategory.service';

@Component({
  selector: 'app-adupdate',
  templateUrl: './adupdate.component.html',
  styleUrls: ['./adupdate.component.css']
})

export class AdupdateComponent implements OnInit {
  baseUrl: string;
  advertFromDb: Advert;
  advertForm: FormGroup;
  allCategoriesFromDb: Category[] = [];
  selectedCategory: Category;
  selectedBrand: string;
  numberOfFiles = 6;
  pictureGUIDs = new Array<string>(this.numberOfFiles);
  uploadedPicturesBinaryData: string[] = [];
  formData: FormData;
  brandCategoryIdForUpdate: number;
  placeholderImageUrl = '/assets/images/camera.jpg';

  constructor(private advertService: AdvertService, private router: Router, private catservice: CatService,
              private toast: ToastrService, private route: ActivatedRoute,
              private brandCatService: BrandCategoryService) { }

  ngOnInit() {
    this.baseUrl = environment.baseUrl;
    this.initForm();
    this.initPlaceHolderImages();
    this.getCats();
    this.loadAdvertFromDb();
    this.formData = new FormData();
  }

  initForm() {
    this.advertForm = new FormGroup({
      title: new FormControl(''),
      description: new FormControl(''),
      price: new FormControl(0),
      brand: new FormControl(''),
      photo: new FormControl(),
      categoryId: new FormControl(0)
    });
  }

  loadAdvertFromDb() {
    this.advertService.getAd(this.getAnnoucementId()).subscribe(advertResponse => {
      this.advertFromDb = advertResponse;
      this.replacePlaceHolderImagesWithRealImages(this.advertFromDb.photoUrls);
      this.brandCatService.getById(this.advertFromDb.brandCategoryId).subscribe(brandCatResponse => {
        this.selectedCategory = this.allCategoriesFromDb.find(x => x.categoryId === brandCatResponse.categoryId);
        this.brandCategoryIdForUpdate = this.advertFromDb.brandCategoryId;
        this.patchFormValues(advertResponse, brandCatResponse);
      });
    });
  }



  replacePlaceHolderImagesWithRealImages(images: string[]) {
    for (let i = 0; i < images.length; i++) {
      this.pictureGUIDs[i] = images[i];
    }
  }

  patchFormValues(advertResponse: Advert, brandCatResponse: BrandCategory) {
    this.advertForm.patchValue({
      title: advertResponse.title,
      description: advertResponse.description,
      price: advertResponse.price,
      categoryId: brandCatResponse.categoryId,
      brand: brandCatResponse.brandTitle,
    });
  }

  getBrandCategoryId(categoryTitle: string, brandTitle: string) {
    let brandCatId;
    this.brandCatService.getBrandCatId(categoryTitle, brandTitle).subscribe(response => {
      brandCatId = response[0].brandCategoryId;
      this.formData.set('brandCategoryId', brandCatId);
    });
  }

  initPlaceHolderImages() {
    for (let i = 0; i < this.numberOfFiles; i++) {
      this.pictureGUIDs[i] = this.placeholderImageUrl;
    }
  }

  getAnnoucementId() {
    return +this.route.snapshot.paramMap.get('id');
  }

  getCats() {
    this.catservice.getAll().subscribe(
      response => {
        this.allCategoriesFromDb = response;
      }, error => {
        this.toast.error(error);
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
        this.pictureGUIDs[i] = event.target.result;
      });
      fileReader.readAsDataURL(file);
    }
  }

  onFileCancel(imageIndex) {
    console.log(imageIndex);
    this.uploadedPicturesBinaryData.splice(imageIndex, 1);
    this.pictureGUIDs.splice(imageIndex, 1);
    this.pictureGUIDs.push(this.placeholderImageUrl);
    const photos = this.formData.getAll('photo');
    photos.splice(imageIndex, 1);
    this.formData.set('photo', null);
    this.uploadedPicturesBinaryData.forEach(element => {
      this.formData.append('photo', element, 'textfilename');
    });
  }

  private resetImages() {
    this.initPlaceHolderImages();
    this.uploadedPicturesBinaryData = [];
    this.formData.set('photo', null);
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
      this.brandCategoryIdForUpdate = response[0].brandCategoryId;
    });
  }

  getCategory(categoryId: number) {
    this.catservice.getById(categoryId).subscribe(response => {
      this.selectedCategory = response;
    });
  }

  toFormData() {
    this.formData.set('annoucementId', `${this.advertFromDb.id}`);
    this.formData.set('title', this.advertForm.value.title);
    this.formData.set('description', this.advertForm.value.description);
    this.formData.set('price', this.advertForm.value.price);
    this.formData.set('brandCategoryId', `${this.brandCategoryIdForUpdate}`);
  }

  submit() {
    this.toFormData();
    console.log(this.formData);
    this.advertService.updateAd(this.formData).subscribe(response => {
      this.router.navigateByUrl('/ads');
    }, error => {
      this.toast.error(error);
    }
    );
  }
}
