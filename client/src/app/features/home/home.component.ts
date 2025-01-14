import { Component } from '@angular/core';
import {CategoriesComponent} from './categories/categories.component';

@Component({
  selector: 'app-home',
  imports: [
    CategoriesComponent
  ],
  templateUrl: './home.component.html',
  standalone: true,
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
