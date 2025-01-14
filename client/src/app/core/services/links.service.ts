import {inject, Injectable, signal} from '@angular/core';
import {environment} from '../../environment/environment.development';
import {HttpClient} from '@angular/common/http';
import {PageResult} from '../../shared/models/page-result';
import {UrlInfo} from '../../shared/models/url-info';
import {LinksParameters} from '../../shared/models/links-params';
import {tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LinksService {
  apiUrl = environment.apiUrl;
  httpClient = inject(HttpClient);

  pageResult = signal<PageResult<UrlInfo>>(null!);

  // createLink(originalUrl: string){
  //   const body = {OriginalUrl: originalUrl};
  //
  //   return this.httpClient.post<UrlInfo>(`${this.apiUrl}url`, body, {
  //     withCredentials: true,
  //   }).pipe(
  //     tap((newLink) => {
  //       this.pageResult.update((currentPageResult) => {
  //         if (!currentPageResult) return null;
  //         return {
  //           ...currentPageResult,
  //           items: [newLink, ...currentPageResult.items],
  //           totalItemsCount: currentPageResult.totalItemsCount + 1,
  //         };
  //       });
  //     })
  //   );
  // }

  createLink(originalUrl: string) {
    const body = { OriginalUrl: originalUrl };

    return this.httpClient.post<UrlInfo>(`${this.apiUrl}url`, body, {
      withCredentials: true,
    }).pipe(
      tap((newLink) => {
        this.pageResult.update((currentPageResult) => {
          if (!currentPageResult) throw new Error('PageResult is not initialized');
          return {
            ...currentPageResult,
            items: [newLink, ...currentPageResult.items],
            totalItemsCount: currentPageResult.totalItemsCount + 1,
          };
        });
      })
    );
  }

  getLinkById(id: string){
    let url = `${this.apiUrl}url/${id}`;

    return this.httpClient.get<UrlInfo>(url, {withCredentials: true});
  }

  getLinksForSpecificUser(linksParameters: LinksParameters){
    let url = this.apiUrl + 'url';
    const params = new URLSearchParams();

    if(linksParameters.paginationParams){
      params.append(`pageSize`, linksParameters.paginationParams.pageSize.toString());
      params.append(`pageNumber`, linksParameters.paginationParams.pageNumber.toString());
    }

    if(linksParameters.searchParam){
      params.append(`searchParam`, linksParameters.searchParam);
    }

    if(linksParameters.selectedSort){
      params.append(`sortBy`, linksParameters.selectedSort.sortBy);
      params.append(`sortDirection`, linksParameters.selectedSort.sortDirection);
    }

    url += `?${params.toString()}`;

    return this.httpClient.get<PageResult<UrlInfo>>(url, {withCredentials: true}).pipe(tap((response)=>{
      this.pageResult.set(response);
    }));
  }

  // deleteLink(id: string){
  //   const url = `${this.apiUrl}url/${id}`;
  //
  //   return this.httpClient.delete(url, {withCredentials: true}).pipe(
  //     tap(()=>{
  //       this.pageResult().items = this.pageResult().items.filter(z=>z.id !== id)
  //     })
  //   );
  // }

  deleteLink(id: string) {
    const url = `${this.apiUrl}url/${id}`;

    return this.httpClient.delete(url, { withCredentials: true }).pipe(
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
