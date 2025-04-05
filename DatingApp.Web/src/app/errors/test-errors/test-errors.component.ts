import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css'
})
export class TestErrorsComponent {
  baseUrl = 'http://localhost:5030/api/'
  private httpClient = inject(HttpClient);

  get400Erorr() {
    this.httpClient.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }

  get404Erorr() {
    this.httpClient.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }

  get401Erorr() {
    this.httpClient.get(this.baseUrl + 'buggy/auth').subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }

  get500Erorr() {
    this.httpClient.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }

  get400ValidationError() {
    this.httpClient.post(this.baseUrl + 'account/register', {}).subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }
}
