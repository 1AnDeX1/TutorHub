import { Component } from '@angular/core';
import { AuthService } from '../../shared/user/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-teacher-profile',
  standalone: false,
  templateUrl: './teacher-profile.component.html',
  styleUrl: '../user-profile.css'
})
export class TeacherProfileComponent {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
