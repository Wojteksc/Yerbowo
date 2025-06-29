import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SocialLoginModule } from '@abacritt/angularx-social-login';
import { SharedModule } from 'src/app/shared/shared.module';

import { HomeComponent } from './components/home/home.component';
import { HomeRoutingModule } from './home-routing.module';

import { HomeService } from './services/home.service';

@NgModule({
  declarations: [
    HomeComponent,
],
imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SocialLoginModule,
    HomeRoutingModule,
    SharedModule
  ],
providers: [
    HomeService
]
})
export class HomeModule {}