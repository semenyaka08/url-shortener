import {computed, inject, Injectable, signal} from '@angular/core';
import {environment} from '../../environment/environment.development';
import {User} from '../../shared/models/user';
import {HttpClient, HttpParams} from '@angular/common/http';
import {catchError, map, Observable, of} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiUrl = environment.apiUrl;
  httpClient = inject(HttpClient);

  currentUser = signal<User | null>(null);

  isAdmin = computed(()=>{
    const roles = this.currentUser()?.roles;
    return Array.isArray(roles) ? roles.includes('Admin') : roles == 'Admin';
  })

  getCurrentUser(): Observable<boolean> {
    return this.httpClient.get<User>(this.apiUrl + 'account/user-info', { withCredentials: true }).pipe(
      map(data => {
        this.currentUser.set(data);
        return true;
      }),
      catchError(() => {
        console.log("Some errors during fetching data about current user");
        return of(false);
      })
    );
  }

  login(values: any){
    let params = new HttpParams();
    params = params.append('useCookies', 'true');

    return this.httpClient.post(this.apiUrl + 'login', values, {params, withCredentials: true});
  }

  register(values: any){
    return this.httpClient.post(this.apiUrl + 'register', values);
  }

  logout(){
    return this.httpClient.post(this.apiUrl + 'account/logout', {}, {withCredentials: true})
  }
}
