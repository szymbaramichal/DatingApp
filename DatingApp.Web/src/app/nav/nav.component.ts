import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { NgIf } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, NgIf, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  private accountService = inject(AccountService);
  loggedIn: boolean = false;
  model: any = {};

  login() {
    this.accountService.login(this.model).subscribe({
      next: res => {
        console.log(res);
        this.loggedIn = true;
      },
      error: err => {
        console.log(err);
      }
    });
  }

  logout() {
    this.loggedIn = false;
  }
}
