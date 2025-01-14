import {Component, inject} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatDialog, MatDialogRef} from '@angular/material/dialog';
import {RegisterComponent} from '../../account/register/register.component';
import {SnackbarService} from '../../../core/services/snackbar.service';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {LinksService} from '../../../core/services/links.service';
import {ShortenedLinkComponent} from '../shortened-link/shortened-link.component';

@Component({
  selector: 'app-create-link',
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatButton,
    MatLabel
  ],
  templateUrl: './create-link.component.html',
  standalone: true,
  styleUrl: './create-link.component.scss'
})
export class CreateLinkComponent {
  private fb = inject(FormBuilder);

  private dialogService = inject(MatDialog);
  private linksService = inject(LinksService);
  private dialogRef = inject(MatDialogRef<RegisterComponent>);
  private snack = inject(SnackbarService);
  protected validationErrors? : string[];
  private urlPattern =
    /^(https?:\/\/)?((([a-zA-Z0-9\-]+\.)+[a-zA-Z]{2,})|localhost)(:\d{2,5})?(\/.*)?$/;
  registerForm = this.fb.group({
    originalUrl: ['', [Validators.required, Validators.pattern(this.urlPattern)]],
  });


  onSubmit(){
    this.linksService.createLink(this.registerForm.value.originalUrl!).subscribe({
      next: (shortenedLink)=>{
        this.snack.success("You successfully created shortened link")
        this.dialogRef.close();
        this.dialogService.open(ShortenedLinkComponent, {
          maxWidth: '500px',
          data: shortenedLink.shortenedUrl
        })
      },
      error: err => this.validationErrors = err
    })
  }
}
