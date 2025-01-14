import {inject, Injectable} from '@angular/core';
import {AccountService} from './account.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {

  accountService = inject(AccountService);

  init(){
    return this.accountService.getCurrentUser();
  }
}
