import { Routes } from '@angular/router';
import {RegisterComponent} from './features/account/register/register.component';
import {LoginComponent} from './features/account/login/login.component';

export const routes: Routes = [
  {path: 'account/register', component: RegisterComponent},
  {path: 'account/login', component: LoginComponent}
];
