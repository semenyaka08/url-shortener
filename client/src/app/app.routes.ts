import { Routes } from '@angular/router';
import {RegisterComponent} from './features/account/register/register.component';
import {LoginComponent} from './features/account/login/login.component';
import {HomeComponent} from './features/home/home.component';
import {LinksComponent} from './features/links/links.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  { path: 'links', component: LinksComponent},
  {path: 'account/register', component: RegisterComponent},
  {path: 'account/login', component: LoginComponent}
];
