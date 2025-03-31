import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  httpClient = inject(HttpClient);
  title = 'Testtttt';
  users: any;

  ngOnInit(): void {
    this.httpClient.get('https://localhost:5001/api/users').subscribe({
      next: res => {
        this.users = res;
      },
      error: err => console.log('sth went wrong'),
      complete: () => console.log('done')
    });
  }

}
