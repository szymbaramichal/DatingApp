import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService { //singleton, created while starting app and disposed when app is shutdown
  httpClient = inject(HttpClient);
  currentUser = signal<User | null>(null);

  baseUrl = environment.apiUrl;

  login(model: any) {
    return this.httpClient.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(response => {
        if(response)
        {
          this.setCurrentUser(response);
        }
      })
    );
  }

  register(model: any) {
    return this.httpClient.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(response => {
        if(response)
        {
          this.setCurrentUser(response);
        }
        return response;
      })
    );
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
