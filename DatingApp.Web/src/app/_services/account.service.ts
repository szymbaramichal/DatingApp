import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService { //singleton, created while starting app and disposed when app is shutdown
  private httpClient = inject(HttpClient);

  baseUrl = 'http://localhost:5030/api/';

  login(model: any) {
    return this.httpClient.post(this.baseUrl + 'account/login', model);
  }
}
