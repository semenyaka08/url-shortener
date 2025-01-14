import {Component, inject, Input, OnChanges, signal} from '@angular/core';
import {UrlInfo} from '../../../shared/models/url-info';
import {LinksService} from '../../../core/services/links.service';
import {MatCard, MatCardActions, MatCardContent, MatCardHeader, MatCardTitle} from '@angular/material/card';
import {DatePipe} from '@angular/common';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-link-details',
  imports: [
    MatCardContent,
    MatCardTitle,
    MatCardHeader,
    MatCard,
    DatePipe,
    MatCardActions,
    MatButton
  ],
  templateUrl: './link-details.component.html',
  standalone: true,
  styleUrl: './link-details.component.scss'
})
export class LinkDetailsComponent implements OnChanges{
  linksService = inject(LinksService);

  @Input() urlId: string | undefined;

  urlInfo = signal<UrlInfo | undefined>(undefined);

  ngOnChanges(): void {
    if (this.urlId) {
      this.loadUrlInfo();
    }
  }

  private loadUrlInfo(): void {
    if (!this.urlId) return;

    this.linksService.getLinkById(this.urlId).subscribe({
      next: (data) => this.urlInfo.set(data),
      error: (err) => console.error(err),
    });
  }
}
