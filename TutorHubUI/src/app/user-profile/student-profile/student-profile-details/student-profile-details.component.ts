import { Component } from '@angular/core';
import { StudentService } from '../../../shared/student/student.service';
import { StudentCreateModel } from '../../../shared/student/studentModels/student-create-model.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../shared/user/auth.service';

@Component({
  selector: 'app-student-profile-details',
  standalone: false,
  templateUrl: './student-profile-details.component.html',
  styles: ``
})
export class StudentProfileDetailsComponent {
  studentId: number | null = null;
  student: StudentCreateModel = new StudentCreateModel();
  isLoading = false;
  isEditing = false;

  constructor(
    private studentService: StudentService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.studentId = this.authService.getStudentId();
    this.loadStudent();
  }

  loadStudent(): void {
    this.isLoading = true;
    this.studentService.getStudentById(this.studentId!).subscribe({
      next: (data) => {
        this.student = data;
        this.isLoading = false;
      },
      error: () => {
        this.toastr.error('Failed to load student details');
        this.isLoading = false;
      }
    });
  }

  enableEdit(): void {
    this.isEditing = true;
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.loadStudent();
  }

  updateStudent(): void {
    this.studentService.updateStudent(this.studentId!, this.student).subscribe({
      next: () => {
        this.toastr.success('Student updated successfully');
        this.isEditing = false;
        this.loadStudent();
      },
      error: () => {
        this.toastr.error('Failed to update student');
      }
    });
  }

  deleteStudent(): void {
    if (confirm('Are you sure you want to delete your profile? This action cannot be undone.')) {
      this.studentService.deleteStudent(this.studentId!).subscribe({
        next: () => {
          this.toastr.success('Student deleted');
          this.router.navigate(['/']); // redirect to home or other page
        },
        error: () => {
          this.toastr.error('Failed to delete student');
        }
      });
    }
  }
}
