import {Component, inject} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {CreateLinkComponent} from './create-link/create-link.component';

@Component({
  selector: 'app-links',
  imports: [],
  templateUrl: './links.component.html',
  standalone: true,
  styleUrl: './links.component.scss'
})
export class LinksComponent {
  private dialogService = inject(MatDialog);

  openCreateLinkDialog() {
    this.dialogService.open(CreateLinkComponent, {
      maxWidth: '500px'
    });
  }
}
