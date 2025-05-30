import { Component } from '@angular/core';
import { AuthService } from '../../shared/user/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-student-profile',
  standalone: false,
  templateUrl: './student-profile.component.html',
  styleUrl: '../user-profile.css'
})
export class StudentProfileComponent {
constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['']);
  }
}
