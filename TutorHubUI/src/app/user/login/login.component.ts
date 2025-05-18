import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../../shared/user/userModels/login-model.model';
import { AuthService } from '../../shared/user/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styles: ``
})
export class LoginComponent implements OnInit {

  loginData: LoginModel = new LoginModel();
  loading = false;

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if(this.authService.isLoggedIn()){
      if(this.authService.getStudentId() != null)
        this.router.navigateByUrl('/student-profile')
      if(this.authService.getTeacherId() != null)
        this.router.navigateByUrl('/teacher-profile')
    }
  }

  onSubmit() {
    this.loading = true;

    this.authService.login(this.loginData).subscribe({
      next: res => {
        this.authService.saveToken(res.token);
        this.toastr.success('Login successful', `Welcome ${res.email}`);
        if(this.authService.getStudentId() != null)
          this.router.navigateByUrl('/student-profile')
        if(this.authService.getTeacherId() != null)
          this.router.navigateByUrl('/teacher-profile')
      },
      error: err => {
        this.toastr.error(err.error || 'Login failed', 'Error');
        this.loading = false;
      }
    });
  }
}
