import {inject, Injectable, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environment/environment.development';
import {LinksParameters} from '../../shared/models/links-params';
import {PageResult} from '../../shared/models/page-result';
import {UrlInfo} from '../../shared/models/url-info';
import {tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);

  pageResult = signal<PageResult<UrlInfo>>(null!);

  getAllLinks(linksParams: LinksParameters) {
    const params = new URLSearchParams();
    if (linksParams.paginationParams) {
      params.append('pageSize', linksParams.paginationParams.pageSize.toString());
      params.append('pageNumber', linksParams.paginationParams.pageNumber.toString());
    }
    if (linksParams.selectedSort) {
      params.append('sortBy', linksParams.selectedSort.sortBy);
      params.append('sortDirection', linksParams.selectedSort.sortDirection);
    }

    const url = `${this.apiUrl}admin/urls?${params.toString()}`;

    return this.httpClient.get<PageResult<UrlInfo>>(url, {withCredentials: true}).pipe(
      tap((response) => {
        this.pageResult.set(response);
      })
    );
  }

  deleteLink(id: string) {
    const url = `${this.apiUrl}url/${id}`;
    return this.httpClient.delete(url, {withCredentials: true}).pipe(
      tap(() => {
        this.pageResult.update((currentPageResult) => {
          if (!currentPageResult) throw new Error('PageResult is not initialized');
          return {
            ...currentPageResult,
            items: currentPageResult.items.filter((item) => item.id !== id),
            totalItemsCount: currentPageResult.totalItemsCount - 1,
          };
        });
      })
    );
  }
}
