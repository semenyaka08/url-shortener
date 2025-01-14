import {Component, Inject} from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogTitle
} from '@angular/material/dialog';
import {MatFormField} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-shortened-link',
  imports: [
    MatFormField,
    MatDialogContent,
    MatDialogTitle,
    MatInput,
    MatDialogActions,
    MatButton,
    MatDialogClose
  ],
  templateUrl: './shortened-link.component.html',
  standalone: true,
  styleUrl: './shortened-link.component.scss'
})
export class ShortenedLinkComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public shortenedLink: string) {}
}
