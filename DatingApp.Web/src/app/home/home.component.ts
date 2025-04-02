import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  private httpClient = inject(HttpClient);
  registerMode = false;
  users: any;
  
  ngOnInit(): void {
    this.getUsers(); 
  }

  registerToggle () {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }

  
  getUsers() {
    this.httpClient.get('http://localhost:5030/api/users').subscribe({
      next: res => {
        this.users = res;
      },
      error: err => console.log('sth went wrong'),
      complete: () => console.log('done')
    });
  }
}
