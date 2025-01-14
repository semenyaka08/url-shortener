import {inject, Injectable} from '@angular/core';
import {environment} from '../../environment/environment.development';
import {HttpClient} from '@angular/common/http';
import {Algorithm} from '../../shared/models/algorithm';

@Injectable({
  providedIn: 'root'
})
export class AlgorithmService {
  apiUrl = environment.apiUrl;
  httpClient = inject(HttpClient);

  updateAlgorithm(algorithm: Algorithm){
    return this.httpClient.post(`${this.apiUrl}algorithm`, algorithm, {withCredentials: true})
  }

  getAlgorithm(){
    return this.httpClient.get<Algorithm>(`${this.apiUrl}algorithm`);
  }
}
