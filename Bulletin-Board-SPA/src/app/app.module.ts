import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { NavComponent } from './MainPages/nav/nav.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AdvertService } from './services/Repositories/advert.service';
import { AdlistComponent } from './MainPages/Advert/adlist/adlist.component';
import { AppRoutingModule } from './app-routing.module';
import { HomeComponent } from './MainPages/home/home.component';
import { CommonModule } from '@angular/common';
import { AddetailComponent } from './MainPages/Advert/addetail/addetail.component';
import { AdcreateComponent } from './MainPages/Advert/adcreate/adcreate.component';
import { AdupdateComponent } from './MainPages/Advert/adupdate/adupdate.component';
import { AuthService } from './services/auth/auth.service';
import { RegistrationComponent } from './MainPages/registration/registration.component';
import { GlobalsService } from './services/global/globals.service';
import { CatService } from './services/Repositories/cat.service';
// import { CatlistComponent } from './Admin/catlist/catlist.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSliderModule } from '@angular/material/slider';
import { ErrorInterceptorService } from './services/err/errorInterceptor.service';
import { ToastrModule } from 'ngx-toastr';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
// import { TownListComponent } from './MainPages/Town/townList/townList.component';
// import { BrandListComponent } from './MainPages/Brand/brandList/brandList.component';
import { JwtTokenInterceptorService } from './services/auth/jwtTokenInterceptor.service';
import { MaterialModule} from './material/material.module';
// import { GenericEntityComponent } from './MainPages/GenericEntity/GenericEntity.component';
import { UserInfoComponent } from './MainPages/Advert/addetail/userInfo/userInfo.component';
import { MessagesComponent } from './MainPages/Messages/messages/messages.component';
import { MessagesTableComponent } from './MainPages/Messages/messagesTable/messagesTable.component';
import { MessageModalComponent } from './MainPages/Messages/messageModal/messageModal.component';
import { UserprofileComponent } from './MainPages/Profile/userprofile/userprofile.component';
import { AdtableComponent } from './MainPages/Advert/adtable/adtable.component';
import { AdtablebodyComponent } from './MainPages/Advert/adtable/adtablebody/adtablebody.component';
import { AdtilesComponent } from './MainPages/Advert/adtable/adtiles/adtiles.component';
import { SidenavComponent } from './MainPages/sidenav/sidenav.component';

export function toketGetter(){
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      AdlistComponent,
      HomeComponent,
      AddetailComponent,
      AdcreateComponent,
      AdupdateComponent,
      RegistrationComponent,
      // CatlistComponent,
      // TownListComponent,
      // BrandListComponent,
      // GenericEntityComponent,
      UserInfoComponent,
      MessagesComponent,
      MessagesTableComponent,
      MessageModalComponent,
      UserprofileComponent,
      AdtableComponent,
      AdtablebodyComponent,
      AdtilesComponent,
      SidenavComponent,
   ],
   imports: [
      CommonModule,
      BrowserModule,
      FormsModule,
      ReactiveFormsModule,
      HttpClientModule,
      AppRoutingModule,
      BrowserAnimationsModule,
      MatSliderModule,
      ToastrModule.forRoot({
         timeOut: 2500,
         positionClass: 'toast-bottom-right',
         preventDuplicates: true}),
      FontAwesomeModule,
      MaterialModule,
   ],
   providers: [
      AdvertService,
      AuthService,
      GlobalsService,
      CatService,
      {
         provide: HTTP_INTERCEPTORS,
         useClass: ErrorInterceptorService,
         multi: true
      },
      {
         provide: HTTP_INTERCEPTORS,
         useClass: JwtTokenInterceptorService,
         multi: true
      }


   ],
   bootstrap: [
      AppComponent
   ],
   entryComponents: [RegistrationComponent]

})
export class AppModule { }
