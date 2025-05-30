import { Component } from '@angular/core';
import { StudentModel } from '../../../shared/student/studentModels/student-model.model';
import { StudentTeacherStatus } from '../../../enums/student-teacher-status.enum';
import { StudentTeacherService } from '../../../shared/student-teacher/student-teacher.service';
import { AuthService } from '../../../shared/user/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-teacher-student',
  standalone: false,
  templateUrl: './teacher-student.component.html',
  styleUrl: '../../user-connection.css'
})
export class TeacherStudentComponent {
  students: StudentModel[] = [];
  currentStatus: StudentTeacherStatus = StudentTeacherStatus.Confirmed;

  StudentTeacherStatus = StudentTeacherStatus;

  constructor(
    private studentTeacherService: StudentTeacherService,
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    const teacherId = +this.authService.getTeacherId()!;
    this.studentTeacherService.getStudentsOfTeacher(teacherId, this.currentStatus).subscribe({
      next: (res) => {
        this.students = res;
      },
      error: () => {
        this.toastr.error('Error loading students');
      }
    });
  }

  toggleStatus(): void {
    this.currentStatus =
      this.currentStatus === StudentTeacherStatus.Confirmed
        ? StudentTeacherStatus.Pending
        : StudentTeacherStatus.Confirmed;

    this.loadStudents();
  }

  getStatusLabel(): string {
    return this.currentStatus === StudentTeacherStatus.Confirmed
      ? 'Show Pending Students'
      : 'Show Confirmed Students';
  }

  confirmStudent(studentId: number): void {
    const teacherId = +this.authService.getTeacherId()!;
    this.studentTeacherService.confirmStudent(teacherId, studentId).subscribe({
      next: () => {
        this.toastr.success('Student confirmed');
        this.loadStudents();
      },
      error: () => {
        this.toastr.error('Error confirming student');
      }
    });
  }

  rejectStudent(studentId: number): void {
    const teacherId = +this.authService.getTeacherId()!;
    this.studentTeacherService.rejectStudent(teacherId, studentId).subscribe({
      next: () => {
        this.toastr.success('Student rejected');
        this.loadStudents();
      },
      error: () => {
        this.toastr.error('Error rejecting student');
      }
    });
  }

  unsubscribeStudent(studentId: number): void {
    const teacherId = +this.authService.getTeacherId()!;
    this.studentTeacherService.unsubscribe(teacherId, studentId).subscribe({
      next: () => {
        this.toastr.success('Student unsubscribed');
        this.loadStudents();
      },
      error: () => {
        this.toastr.error('Error unsubscribing student');
      }
    });
  }
}
