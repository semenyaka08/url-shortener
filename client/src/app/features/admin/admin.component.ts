import {AfterViewInit, Component, computed, effect, inject, OnInit, ViewChild} from '@angular/core';
import {
  MatCell, MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef, MatHeaderRow, MatHeaderRowDef, MatRow, MatRowDef,
  MatTable,
  MatTableDataSource
} from '@angular/material/table';
import {UrlInfo} from '../../shared/models/url-info';
import {LinksParameters} from '../../shared/models/links-params';
import {AdminService} from '../../core/services/admin.service';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import {MatIcon} from '@angular/material/icon';
import {MatIconButton} from '@angular/material/button';
import {DatePipe, NgIf} from '@angular/common';
import {Router} from '@angular/router';

@Component({
  selector: 'app-admin',
  imports: [
    MatPaginator,
    MatTable,
    MatColumnDef,
    MatHeaderCell,
    MatHeaderCellDef,
    MatCell,
    MatCellDef,
    MatIcon,
    MatIconButton,
    MatHeaderRow,
    MatRow,
    MatRowDef,
    MatHeaderRowDef,
    NgIf,
    DatePipe
  ],
  templateUrl: './admin.component.html',
  standalone: true,
  styleUrl: './admin.component.scss'
})
export class AdminComponent implements OnInit, AfterViewInit{
  displayedColumns: string[] = ['id', 'shortenedUrl', 'originalUrl', 'userEmail', 'createdAt', 'actions'];
  dataSource = new MatTableDataSource<UrlInfo>([]);
  private adminService = inject(AdminService);
  private router = inject(Router);

  linksParams: LinksParameters = {
    selectedSort: {sortBy: 'id', sortDirection: 'asc'},
    paginationParams: {pageNumber: 1, pageSize: 4},
  };

  pageResult = computed(() => this.adminService.pageResult());

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  constructor() {
    effect(() => {
      this.dataSource.data = [...this.pageResult().items];
    });
  }

  ngOnInit(): void {
    this.loadLinks();
  }

  loadLinks() {
    this.adminService.getAllLinks(this.linksParams).subscribe({
      next: () => {},
      error: (err) => console.error(err),
    });
  }

  onPageEvent($event: PageEvent) {
    this.linksParams.paginationParams!.pageNumber = $event.pageIndex + 1;
    this.linksParams.paginationParams!.pageSize = $event.pageSize;
    this.loadLinks();
  }

  deleteUrl(id: string) {
    this.adminService.deleteLink(id).subscribe({
      next: () => console.log('Link deleted'),
      error: (err) => console.error(err),
    });
  }

  goToUrlDetails(id: string) {
    this.router.navigate([`/links/`, id]);
  }

  onLinkClick(event: MouseEvent, url: string) {
    event.stopPropagation();
    window.open(url, '_blank');
  }
}
