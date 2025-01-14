import { Routes } from '@angular/router';
import {RegisterComponent} from './features/account/register/register.component';
import {LoginComponent} from './features/account/login/login.component';
import {HomeComponent} from './features/home/home.component';
import {LinksComponent} from './features/links/links.component';
import {LinkDetailsComponent} from './features/links/link-details/link-details.component';
import {AlgorithmComponent} from './features/algorithm/algorithm.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  { path: 'links', component: LinksComponent},
  { path: 'links/:urlId', component: LinkDetailsComponent},
  {path: 'account/register', component: RegisterComponent},
  {path: 'account/login', component: LoginComponent},
  {path: 'algorithm', component: AlgorithmComponent}
];
