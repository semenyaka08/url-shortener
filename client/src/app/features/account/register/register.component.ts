import {Component, inject} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AccountService} from '../../../core/services/account.service';
import {SnackbarService} from '../../../core/services/snackbar.service';
import {Router} from '@angular/router';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatCard} from '@angular/material/card';
import {MatButton} from '@angular/material/button';
import {MatInput} from '@angular/material/input';

@Component({
  selector: 'app-register',
  imports: [
    MatLabel,
    MatFormField,
    ReactiveFormsModule,
    MatCard,
    MatButton,
    MatInput
  ],
  templateUrl: './register.component.html',
  standalone: true,
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);

  private accountService = inject(AccountService);
  private snack = inject(SnackbarService);
  private router = inject(Router);
  protected validationErrors? : string[];

  registerForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  async onSubmit(){
    this.accountService.register(this.registerForm.value).subscribe({
      next: ()=>{
        this.snack.success("Registration successful - you can now login");
        this.router.navigateByUrl('');
      },
      error: err => this.validationErrors = err
    })
  }
}
