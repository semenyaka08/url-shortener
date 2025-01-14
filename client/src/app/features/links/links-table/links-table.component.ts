import {AfterViewInit, Component, inject, OnInit, ViewChild} from '@angular/core';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatSelect} from '@angular/material/select';
import {
  MatCell, MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef, MatHeaderRow, MatHeaderRowDef, MatRow, MatRowDef,
  MatTable,
  MatTableDataSource
} from '@angular/material/table';
import {UrlInfo} from '../../../shared/models/url-info';
import {LinksService} from '../../../core/services/links.service';
import {LinksParameters} from '../../../shared/models/links-params';
import {PageResult} from '../../../shared/models/page-result';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import {DatePipe, NgIf} from '@angular/common';
import {MatIcon} from '@angular/material/icon';
import {MatIconButton} from '@angular/material/button';

@Component({
  selector: 'app-links-table',
  imports: [
    MatFormField,
    MatLabel,
    MatSelect,
    MatTable,
    MatColumnDef,
    MatHeaderCell,
    MatHeaderCellDef,
    MatCell,
    MatCellDef,
    DatePipe,
    MatHeaderRow,
    MatRow,
    MatRowDef,
    MatHeaderRowDef,
    MatPaginator,
    NgIf,
    MatIcon,
    MatIconButton
  ],
  templateUrl: './links-table.component.html',
  standalone: true,
  styleUrl: './links-table.component.scss'
})
export class LinksTableComponent implements OnInit, AfterViewInit{
  displayedColumns: string[] = ['shortenedUrl', 'originalUrl', 'createdAt', 'actions'];
  dataSource = new MatTableDataSource<UrlInfo>([]);
  linksService = inject(LinksService);
  linksParameters: LinksParameters = {
    selectedSort: {sortBy: 'id', sortDirection: 'asc'},
    paginationParams: {pageNumber: 1, pageSize: 4},
  };

  pageResult!: PageResult<UrlInfo>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ngOnInit(): void {
    this.loadLinks();
  }

  loadLinks() {
    this.linksService.getLinksForSpecificUser(this.linksParameters).subscribe({
      next: (data) => {
        this.pageResult = data;
        this.dataSource.data = data.items;},
      error: (err) => {console.log(err)}
    });
  }

  onPageEvent($event: PageEvent) {
    this.linksParameters.paginationParams!.pageNumber = $event.pageIndex + 1;
    this.linksParameters.paginationParams!.pageSize = $event.pageSize;
    this.loadLinks();
  }

  deleteUrl(id: string) {
    this.linksService.deleteLink(id).subscribe({
      next: ()=>{
        console.log("Link was deleted");
        this.loadLinks();
      },
      error: (err)=>{
        console.error("Error deleting the link:", err);
      }
    })
  }
}
