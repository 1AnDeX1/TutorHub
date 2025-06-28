import { Component, OnInit } from "@angular/core";
import { TeacherModel } from "../../../shared/teacher/teacherModels/teacher-model.model";
import { Subject } from "../../../enums/subject.enum";
import { VerificationStatus } from "../../../enums/verification-status.enum";
import { DayOfWeek } from "../../../enums/day-of-week.enum";
import { AuthService } from "../../../shared/user/auth.service";
import { ActivatedRoute, Router } from "@angular/router";
import { TeacherService } from "../../../shared/teacher/teacher.service";
import { ToastrService } from "ngx-toastr";
import { TeacherCreateModel } from "../../../shared/teacher/teacherModels/teacher-create-model.model";

@Component({
  selector: 'app-teacher-profile-details',
  standalone: false,
  templateUrl: './teacher-profile-details.component.html',
  styleUrl: '../../user-profile-details.css'
})
export class TeacherProfileDetailsComponent implements OnInit {
  teacherId: number | null = null;
  teacher: TeacherCreateModel = new TeacherCreateModel();
  isLoading = false;
  isEditing = false;
  Subject = Subject;
  verificationEnum!: VerificationStatus;
  VerificationStatus = VerificationStatus;
  subjectKeys: string[] = [];
  emailAddress = 'verification@email.com';
  showVerificationRequest = false;

  constructor(
    private teacherService: TeacherService,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.subjectKeys = Object.keys(Subject).filter(k => isNaN(Number(k)));
  }

  ngOnInit(): void {
    this.teacherId = this.authService.getTeacherId();
    if (this.teacherId) {
      this.loadTeacher();
    } else {
      this.toastr.error('No teacher ID found');
      this.router.navigate(['/']);
    }
  }

  loadTeacher(): void {
    this.isLoading = true;
    this.teacherService.getTeacherById(this.teacherId!).subscribe({
      next: (data) => {
        this.teacher = { ...data, password: ''};
        this.verificationEnum = VerificationStatus[data.verificationStatus as unknown as keyof typeof VerificationStatus];
        this.isLoading = false;
      },
      error: () => {
        this.toastr.error('Failed to load teacher details');
        this.isLoading = false;
      }
    });
  }

  enableEdit(): void {
    this.isEditing = true;
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.loadTeacher();
  }

  updateTeacher(): void {
    this.teacherService.putTeacher(this.teacherId!, this.teacher).subscribe({
      next: () => {
        this.toastr.success('Teacher updated successfully');
        this.isEditing = false;
        this.loadTeacher();
      },
      error: () => {
        this.toastr.error('Failed to update teacher');
      }
    });
  }

  deleteTeacher(): void {
    if (confirm('Are you sure you want to delete your profile?')) {
      this.teacherService.deleteTeacher(this.teacherId!).subscribe({
        next: () => {
          this.toastr.success('Teacher deleted');
          this.router.navigate(['/']);
        },
        error: () => {
          this.toastr.error('Failed to delete teacher');
        }
      });
    }
  }

  toggleSubject(subjectName: string): void {
    const subjectEnum = Subject[subjectName as keyof typeof Subject];
    const index = this.teacher.subjects.indexOf(subjectEnum);

    if (index === -1) {
      this.teacher.subjects.push(subjectEnum);
    } else {
      this.teacher.subjects.splice(index, 1);
    }
  }

  isSubjectSelected(subjectName: string): boolean {
    const subjectEnum = Subject[subjectName as keyof typeof Subject];
    return this.teacher.subjects.includes(subjectEnum);
  }

  requestVerification(): void {
    if (!this.teacherId) return;

    this.teacherService.requestVerification(this.teacherId).subscribe({
      next: () => {
        this.toastr.success('Verification request sent successfully.');
        this.loadTeacher();
      },
      error: () => {
        this.toastr.error('Failed to send verification request.');
      }
    });
  }

}