import {Component, inject} from '@angular/core';
import {AccountService} from '../../core/services/account.service';
import {MatMenu, MatMenuItem, MatMenuTrigger} from '@angular/material/menu';
import {MatIcon} from '@angular/material/icon';
import {RouterLink} from '@angular/router';
import {MatButton} from '@angular/material/button';
import {MatDivider} from '@angular/material/divider';
import {IsAdminDirective} from '../../shared/directives/is-admin.directive';

@Component({
  selector: 'app-header',
  imports: [
    MatMenuTrigger,
    MatIcon,
    MatMenu,
    RouterLink,
    MatButton,
    MatMenuItem,
    MatDivider,
    IsAdminDirective
  ],
  templateUrl: './header.component.html',
  standalone: true,
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  protected accountService = inject(AccountService);

  logout() {
    this.accountService.logout().subscribe({
      next: ()=> this.accountService.currentUser.set(null)
    });
  }
}
