import {Component, inject, Inject} from '@angular/core';
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
import {SnackbarService} from '../../../core/services/snackbar.service';

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

  snackbarService = inject(SnackbarService);

  copyToClipboard(value: string): void {
    navigator.clipboard.writeText(value).then(() => {
      this.snackbarService.success('Link copied to clipboard!')
    });
  }
}
