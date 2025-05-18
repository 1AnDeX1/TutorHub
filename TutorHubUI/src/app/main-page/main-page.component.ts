import { Component, OnInit } from '@angular/core';
import { TeacherService } from '../shared/teacher/teacher.service';
import { ToastrService } from 'ngx-toastr';
import { Subject } from '../enums/subject.enum';
import { VerificationStatus } from '../enums/verification-status.enum';

@Component({
  selector: 'app-main-page',
  standalone: false,
  templateUrl: './main-page.component.html',
  styles: ``
})
export class MainPageComponent implements OnInit {

  subjectEnum = Subject;
  verificationEnum = VerificationStatus;

  constructor(
    public teacherService: TeacherService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.teacherService.getTeachers().subscribe({
      next: res => this.teacherService.list = res.teachers,
      error: err => this.toastr.error('Failed to load teachers')
    });
  }

  deleteTeacher(id: number) {
    if (confirm("Are you sure?")) {
      this.teacherService.deleteTeacher(id).subscribe({
        next: () => {
          this.toastr.success('Deleted successfully', "Profile");
          // Refresh list after deletion
          this.teacherService.getTeachers();
        },
        error: err => {
          this.toastr.error(err.error || 'Error deleting teacher', 'Error');
        }
      });
    }
  }
}
