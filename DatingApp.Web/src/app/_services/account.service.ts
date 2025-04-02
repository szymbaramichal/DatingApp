import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService { //singleton, created while starting app and disposed when app is shutdown
  httpClient = inject(HttpClient);
  currentUser = signal<User | null>(null);

  baseUrl = 'http://localhost:5030/api/';

  login(model: any) {
    return this.httpClient.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(response => {
        if(response)
        {
          localStorage.setItem('user', JSON.stringify(response));
          this.currentUser.set(response)
        }
      })
    );
  }

  register(model: any) {
    return this.httpClient.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(response => {
        if(response)
        {
          localStorage.setItem('user', JSON.stringify(response));
          this.currentUser.set(response);
        }
        return response;
      })
    );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
