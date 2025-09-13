import { Component, inject, signal } from '@angular/core';
import { Register } from '../account/register/register';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  protected registerMode = signal(false);
  protected acountService = inject(AccountService);

  showRegister(value: boolean) {
    this.registerMode.set(value);
  }
}
