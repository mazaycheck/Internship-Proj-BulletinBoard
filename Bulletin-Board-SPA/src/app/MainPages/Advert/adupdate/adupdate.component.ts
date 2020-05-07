import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AdvertService } from 'src/app/services/Repositories/advert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CatService } from 'src/app/services/Repositories/cat.service';
import { Category } from 'src/app/Models/Category';
import { ToastrService } from 'ngx-toastr';
import { Town } from 'src/app/Models/Town';
import { TownService } from 'src/app/services/Repositories/town.service';
import { Brand } from 'src/app/Models/brand';
import { Advert } from 'src/app/Models/advert';
import { BrandService } from 'src/app/services/Repositories/brand.service';
import { environment } from 'src/environments/environment';
import { BrandCategory } from 'src/app/Models/BrandCategory';
import { BrandCategoryService } from 'src/app/services/Repositories/brandCategory.service';

@Component({
  selector: 'app-adupdate',
  templateUrl: './adupdate.component.html',
  styleUrls: ['./adupdate.component.css']
})

export class AdupdateComponent implements OnInit {
  ngOnInit(): void {
    
  }


  
  // baseUrl: string;
  // id: number;
  // advertFromDb: Advert;
  // advertForm: FormGroup;
  // categories: Category[] = [];
  // brands: Brand[] = [];
  // selectedCategory: Category;
  // numberOfFiles: number;
  // imgUrls = new Array<string>(this.numberOfFiles);
  // pictures: string[] = [];
  // formData: FormData;
  // brand: Brand;
  // brandsOfCategory: BrandCategory[] = [];
  // constructor(private advertService: AdvertService, private router: Router, private catservice: CatService,
  //   private toast: ToastrService, private townservice: TownService, private brandService: BrandService,
  //   private route: ActivatedRoute, private brandCatService: BrandCategoryService) { }

  // ngOnInit() {
  //   this.baseUrl = environment.baseUrl;
  //   this.advertForm = new FormGroup({
  //     title: new FormControl(''),
  //     description: new FormControl(''),
  //     price: new FormControl(0),
  //     brandCategoryId: new FormControl(0),
  //     categoryId: new FormControl(0),
  //     photo: new FormControl()
  //   });

  //   this.id = +this.route.snapshot.paramMap.get('id');
  //   this.advertService.getAd(this.id).subscribe(response => {
  //     this.advertFromDb = response;
  //     this.brandCatService.getById(this.advertFromDb.brandCategoryId).subscribe(response => {
  //       this.getBrandsOfCategory(response.categoryId);
  //       this.advertForm.patchValue({
  //         title: this.advertFromDb.title,
  //         description: this.advertFromDb.description,
  //         price: this.advertFromDb.price,
  //         brandCategoryId: this.advertFromDb.brandCategoryId,
  //         categoryId: response.categoryId
  //       });
  //     });
  //     this.imgUrls = this.advertFromDb.photoUrls;
  //   });

  //   this.numberOfFiles = 3;
  //   this.getCats();
  //   this.getBrands();
  //   for (let i = 0; i < this.numberOfFiles; i++) {
  //     this.imgUrls[i] = '/assets/images/random.jpg';
  //   }
  //   this.formData = new FormData();
  // }

  // getCats() {
  //   this.catservice.getAll().subscribe(
  //     response => {
  //       this.categories = response;
  //     }, error => {
  //       this.toast.error(error);
  //     }
  //   );
  // }


  // getBrandCategoryInfo(id: number) {
  //   this.brandService.getById(id).subscribe(
  //     response => {
  //       this.brand = response;
  //     }
  //   )
  // }



  // getBrands() {
  //   this.brandService.getAll().subscribe(
  //     response => {
  //       this.brands = response;
  //     }, error => {
  //       this.toast.error(error);
  //     }
  //   );
  // }

  // getBrandsFromCategory(categoryId: number) {
  //   this.brandService.getAllFromCategory(categoryId).subscribe(
  //     response => {
  //       this.brands = response;
  //     }, error => {
  //       this.toast.error(error);
  //     }
  //   );
  // }

  // onFileAttach($event) {
  //   for (let i = 0; i < $event.target.files.length; i++) {
  //     const file = $event.target.files[i];
  //     this.pictures.push(file);
  //     // Asp.net core has to accept [FromForm] MyAnnoucementDto
  //     // MyAnnoucementDto has to have List<IFormFile>
  //     this.formData.append('photo', file, file.name);
  //     const fileReader = new FileReader();
  //     fileReader.onload = ((event: any) => {
  //       this.imgUrls[i] = event.target.result;
  //       // Asp.net core has to accept [FromForm] IFormCollection to work for these options:
  //       //this.formData.append(`photo[${i}]`, file);
  //       // OR
  //       //this.formData.append(`photo[]`, file);
  //     });
  //     fileReader.readAsDataURL(file);
  //   }
  // }



  // selectChanged($event) {
  //   const categoryId = $event.value;
  //   if (categoryId > 0) {
  //     this.getBrandsOfCategory(categoryId);
  //   }
  // }

  // getBrandNameById(brandId: number) {
  //   return this.brands.find(x => x.brandId === brandId).title;
  // }

  // toFormData() {
  //   this.formData.append('title', this.advertForm.value.title);
  //   this.formData.append('description', this.advertForm.value.description);
  //   this.formData.append('price', this.advertForm.value.price);
  //   this.formData.append('brandCategoryId', this.advertForm.value.brandCategoryId);
  //   this.formData.append('userId', `${this.advertFromDb.userId}`);
  //   this.formData.append('annoucementId', `${this.advertFromDb.id}`);
  // }

  // submit() {
  //   this.toFormData();
  //   console.log(this.formData);
  //   this.advertService.updateAd(this.formData).subscribe(response => {
  //     console.log(response);
  //     this.router.navigateByUrl('/ads');
  //   }, error => {
  //     this.toast.error(error);
  //   }
  //   );
  // }
}
