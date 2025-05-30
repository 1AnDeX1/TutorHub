import { Component } from '@angular/core';
import { TeacherModel } from '../../shared/teacher/teacherModels/teacher-model.model';
import { Subject } from '../../enums/subject.enum';
import { VerificationStatus } from '../../enums/verification-status.enum';
import { ActivatedRoute } from '@angular/router';
import { TeacherService } from '../../shared/teacher/teacher.service';
import { ScheduleSimpleModel } from '../../shared/schedule/scheduleModels/schedule-simple-model.model';
import { StudentTeacherService } from '../../shared/student-teacher/student-teacher.service';
import { AuthService } from '../../shared/user/auth.service';
import { StudentTeacherRequestModel } from '../../shared/student-teacher/studentTeacherModels/student-teacher-request-model.model';
import { ToastrService } from 'ngx-toastr';
import { DayOfWeek } from '../../enums/day-of-week.enum';
import { TeacherAvailabilitiesComponent } from '../../user-profile/teacher-profile/teacher-availabilities/teacher-availabilities.component';
import { TeacherAvailabilityService } from '../../shared/teacher/teacher-availability.service';
import { TeacherAvailabilityModel } from '../../shared/teacher/teacher-availability-models/teacher-availability-model.model';
import { claimReq } from '../../shared/utils/claimReq-utils';

@Component({
  selector: 'app-teacher-details',
  standalone: false,
  templateUrl: './teacher-details.component.html',
  styleUrl: './teacher-details.component.css'
})
export class TeacherDetailsComponent {
  claimReq = claimReq; // Claim requirements for RBAC
  teacherId!: number;
  teacher!: TeacherModel;
  subjectEnum = Subject;
  verificationEnum = VerificationStatus;
  schedules: ScheduleSimpleModel[] = [];
  availabilities: TeacherAvailabilityModel[] = [];
  groupedAvailabilities: { [key: string]: TeacherAvailabilityModel[] } = {};
  DayOfWeek = DayOfWeek;
  dayOfWeekEnum = Object.keys(DayOfWeek).filter(v => isNaN(Number(v)));
  showConnectionSection = false; // New state variable to toggle the connection section
  dayKeys = Object.keys(DayOfWeek)
    .filter(k => !isNaN(+k))
    .map(k => +k);
    
  constructor(
    private studentTeacherService: StudentTeacherService,
    public authService: AuthService,
    private route: ActivatedRoute,
    private teacherService: TeacherService,
    private availabilityService: TeacherAvailabilityService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.teacherId = +this.route.snapshot.paramMap.get('id')!;
    this.teacherService.getTeacherById(this.teacherId).subscribe({
      next: (res) => {
        this.teacher = res;
      },
      error: (err) => {
        console.error('Teacher not found', err);
      }
    });
    if (this.teacherId) {
      this.loadAvailabilities(this.teacherId);
    }
  }

  addSchedule() {
    this.schedules.push({
      dayOfWeek: 1,
      startTime: '',
      endTime: ''
    });
  }

  removeSchedule(index: number) {
    this.schedules.splice(index, 1);
  }

  submitRequest() {
    const studentId = +this.authService.getStudentId()!;
    const request: StudentTeacherRequestModel = {
      studentId,
      teacherId: this.teacherId,
      schedules: this.schedules
    };

    this.studentTeacherService.requestStudentToTeacher(request).subscribe({
      next: () => this.toastr.success('Request sent successfully'),
      error: () => this.toastr.error('Failed to send request')
    });
  }

  loadAvailabilities(id: number) {
    this.availabilityService.getAvailabilitiesByTeacherId(id)
      .subscribe({
        next: (res) => {
        this.availabilities = res;
        this.groupAvailabilitiesByDay();
      },
        error: err => console.error('Error loading availability', err)
      });
  }

  groupAvailabilitiesByDay(): void {
    this.groupedAvailabilities = {};

    for (const day of this.dayOfWeekEnum) {
      this.groupedAvailabilities[day] = [];
    }

    for (const slot of this.availabilities) {
      const dayName = slot.dayOfWeek;
      if (!this.groupedAvailabilities[dayName]) {
        this.groupedAvailabilities[dayName] = [];
      }
      this.groupedAvailabilities[dayName].push(slot);
    }
  }

  getDayName(day: number): string {
    return DayOfWeek[day];
  }

  startConnectionRequest() {
    this.showConnectionSection = true;
  }

  cancelConnectionRequest() {
    this.showConnectionSection = false;
  }
}
