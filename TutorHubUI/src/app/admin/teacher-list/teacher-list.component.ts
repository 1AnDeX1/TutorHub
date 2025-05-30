import { Component } from '@angular/core';
import { TeacherService } from '../../shared/teacher/teacher.service';
import { ToastrService } from 'ngx-toastr';
import { Subject } from '../../enums/subject.enum';
import { VerificationStatus } from '../../enums/verification-status.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'app-teacher-list',
  standalone: false,
  templateUrl: './teacher-list.component.html',
  styles: ``
})
export class TeacherListComponent {
  isLoading = false;
  subjectEnum = Subject;
  verificationEnum = VerificationStatus;

  constructor(
    public teacherService: TeacherService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadTeachers();
  }

  loadTeachers(): void {
    this.isLoading = true;
    this.teacherService.getTeachers().subscribe({
      next: (res) => {
        this.teacherService.list = res.teachers;
        this.isLoading = false;
      },
      error: () => {
        this.toastr.error('Failed to load teachers');
        this.isLoading = false;
      },
    });
  }

  deleteTeacher(id: number): void {
    if (confirm('Are you sure you want to delete this teacher?')) {
      this.teacherService.deleteTeacher(id).subscribe({
        next: () => {
          this.toastr.success('Teacher deleted successfully');
          this.loadTeachers();
        },
        error: () => {
          this.toastr.error('Failed to delete teacher');
        },
      });
    }
  }

  getSubjectName(subject: Subject): string {
    return this.subjectEnum[subject] || 'Unknown';
  }

  getVerificationStatusName(status: VerificationStatus | undefined): string {
    return status !== undefined ? this.verificationEnum[status] : 'Unknown';
  }
}
