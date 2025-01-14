import {inject, Injectable} from '@angular/core';
import {environment} from '../../environment/environment.development';
import {HttpClient} from '@angular/common/http';

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
}
