import { Component, OnInit } from '@angular/core';
import { TeacherService } from '../shared/teacher/teacher.service';
import { Subject } from '../enums/subject.enum';
import { VerificationStatus } from '../enums/verification-status.enum';
import { TeacherFilter } from '../shared/teacher/teacherModels/teacher-filter.model';

@Component({
  selector: 'app-main-page',
  standalone: false,
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent implements OnInit {
  filter: TeacherFilter = {
    subjects: [],
    minHourlyRate: null,
    maxHourlyRate: null,
    online: null,
    offline: null,
    minAge: null,
    maxAge: null,
    verificationStatus: null,
    page: 1,
    pageSize: 20
  };

  totalPages: number = 0;

  subjectOptions = Object.keys(Subject)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ key, value: Subject[key as keyof typeof Subject] }));

  verificationOptions = Object.keys(VerificationStatus)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ key, value: VerificationStatus[key as keyof typeof VerificationStatus] }));

  subjectEnum = Subject;
  verificationEnum = VerificationStatus;

  constructor(public teacherService: TeacherService) {}

  ngOnInit(): void {
    this.getTeachers();
  }

  getTeachers(): void {
    this.teacherService.getAvailableTeachers(this.filter).subscribe({
      next: (res) => {
        this.teacherService.list = res.teachers;
        this.totalPages = res.totalPages;
        this.filter.page = res.currentPage;
      },
      error: (err) => console.error(err)
    });
  }

  onSubjectToggle(subject: Subject, event: Event): void {
    const input = event.target as HTMLInputElement;
    const isChecked = input.checked;

    if (isChecked) {
      if (!this.filter.subjects!.includes(subject)) {
        this.filter.subjects!.push(subject);
      }
    } else {
      this.filter.subjects = this.filter.subjects!.filter(s => s !== subject);
    }

    this.getTeachers();
  }


  onVerificationChange(event: any): void {
    const value = event.target.value;
    this.filter.verificationStatus = value === '' ? null : +value;
  }

  applyFilters(): void {
    this.getTeachers();
  }

  prevPage(): void {
    if (this.filter.page > 1) {
      this.filter.page--;
      this.getTeachers();
    }
  }

  nextPage(): void {
    if (this.filter.page < this.totalPages) {
      this.filter.page++;
      this.getTeachers();
    }
  }
}
