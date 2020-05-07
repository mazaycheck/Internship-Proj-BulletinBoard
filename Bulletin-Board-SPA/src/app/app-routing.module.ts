import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AdlistComponent } from './MainPages/Advert/adlist/adlist.component';
import { AddetailComponent } from './MainPages/Advert/addetail/addetail.component';

import { AdupdateComponent } from './MainPages/Advert/adupdate/adupdate.component';
import { HomeComponent } from './MainPages/home/home.component';
import { AdcreateComponent } from './MainPages/Advert/adcreate/adcreate.component';
import { RegistrationComponent } from './MainPages/registration/registration.component';

import { AuthGuard } from './services/guards/auth.guard';


import { AdvertResolver } from './services/resolvers/advertResolver';
import { MessagesComponent } from './MainPages/Messages/messages/messages.component';
import { UserprofileComponent } from './MainPages/Profile/userprofile/userprofile.component';
import { AdminGuard } from './services/guards/admin.guard';




export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '', runGuardsAndResolvers: 'always', canActivate: [AuthGuard], children: [
      { path: 'ads', component: AdlistComponent },
      { path: 'newadvert', component: AdcreateComponent },
      { path: 'ads/details/:id', component: AddetailComponent, resolve: { advert: AdvertResolver } },
      { path: 'ads/update/:id', component: AdupdateComponent },
      { path: 'messages', component: MessagesComponent, },
      { path: 'profile/:id', component: UserprofileComponent, },
    ]
  },
  {
    path: 'admin', loadChildren: () => import('./Admin/Admin.module').then(m => m.AdminModule),
    canActivate: [AdminGuard]
  },
  { path: 'auth/register', component: RegistrationComponent },
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
