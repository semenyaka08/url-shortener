import {inject, Injectable} from '@angular/core';
import {environment} from '../../environment/environment.development';
import {HttpClient} from '@angular/common/http';
import {PageResult} from '../../shared/models/page-result';
import {UrlInfo} from '../../shared/models/url-info';
import {LinksParameters} from '../../shared/models/links-params';

@Injectable({
  providedIn: 'root'
})
export class LinksService {
  apiUrl = environment.apiUrl;
  httpClient = inject(HttpClient);

  createLink(originalUrl: string){
    const body = {OriginalUrl: originalUrl};

    return this.httpClient.post<string>(`${this.apiUrl}url`, body,
      {
        responseType: 'text' as 'json',
        withCredentials: true});
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

    return this.httpClient.get<PageResult<UrlInfo>>(url, {withCredentials: true});
  }

  deleteLink(id: string){
    const url = `${this.apiUrl}url/${id}`;

    return this.httpClient.delete(url, {withCredentials: true});
  }
}
