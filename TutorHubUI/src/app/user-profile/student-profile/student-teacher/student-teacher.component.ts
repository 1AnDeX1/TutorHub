import { Component } from '@angular/core';
import { TeacherModel } from '../../../shared/teacher/teacherModels/teacher-model.model';
import { StudentTeacherService } from '../../../shared/student-teacher/student-teacher.service';
import { AuthService } from '../../../shared/user/auth.service';
import { ToastrService } from 'ngx-toastr';
import { StudentTeacherStatus } from '../../../enums/student-teacher-status.enum';
import { Subject } from '../../../enums/subject.enum';
import { TeacherRatingModel } from '../../../shared/teacher/teacherModels/teacher-rating-model.model';
import { TeacherService } from '../../../shared/teacher/teacher.service';

@Component({
  selector: 'app-student-teacher',
  standalone: false,
  templateUrl: './student-teacher.component.html',
  styleUrl: '../../user-connection.css'
})
export class StudentTeacherComponent {
  teachers: TeacherModel[] = [];
  Subject = Subject;
  ratingValues: { [teacherId: number]: number } = {};

  constructor(
    private studentTeacherService: StudentTeacherService,
    private teacherService: TeacherService,
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadTeachers();
  }

  loadTeachers(): void {
    const studentId = +this.authService.getStudentId()!;
    this.studentTeacherService
      .getTeachersOfStudent(studentId, StudentTeacherStatus.Confirmed)
      .subscribe({
        next: (res) => {
          this.teachers = res;
        },
        error: () => {
          this.toastr.error('Failed to load teachers');
        }
      });
  }

  unsubscribe(teacherId: number): void {
    const studentId = +this.authService.getStudentId()!;
    this.studentTeacherService.unsubscribe(teacherId, studentId).subscribe({
      next: () => {
        this.toastr.success('Unsubscribed from teacher');
        this.loadTeachers();
      },
      error: () => {
        this.toastr.error('Failed to unsubscribe');
      }
    });
  }

  rateTeacher(teacherId: number): void {
    const studentId = +this.authService.getStudentId()!;
    const value = this.ratingValues[teacherId];

    if (!value || value < 1 || value > 5) {
      this.toastr.error('Please select a rating between 1 and 5');
      return;
    }

    const rating: TeacherRatingModel = {
      studentId,
      teacherId,
      value
    };

    this.teacherService.rateTeacher(rating).subscribe({
      next: (res) => {
        console.log('Success response:', res);
        this.toastr.success('Teacher rated successfully');
        this.ratingValues[teacherId] = 0;
        this.loadTeachers();
      },
      error: (err) => {
        console.error('Rating error:', err);
        this.toastr.error('Failed to rate teacher');
      }
    });
  }
}
