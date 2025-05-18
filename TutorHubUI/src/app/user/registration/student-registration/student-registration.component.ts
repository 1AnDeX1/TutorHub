import { Component } from '@angular/core';
import { StudentService } from '../../../shared/student/student.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-student-registration',
  standalone: false,
  templateUrl: './student-registration.component.html',
  styles: ``
})
export class StudentRegistrationComponent {
  
  constructor(
    public studentService: StudentService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  onSubmit(form: NgForm) {
    this.studentService.formSubmitted = true;

    if (form.valid) {
      this.studentService.postStudent().subscribe({
        next: () => {
          this.toastr.success('Student created successfully', 'Success');
          this.studentService.resetForm(form);
          this.router.navigate(['/main']);
        },
        error: err => {
          this.toastr.error(err.error || 'Server error', 'Creation Failed');
        }
      });
    } else {
      this.toastr.error('Please fix the form errors', 'Validation Error');
    }
  }
}
