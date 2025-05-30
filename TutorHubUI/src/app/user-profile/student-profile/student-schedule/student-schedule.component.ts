import { Component } from '@angular/core';
import { ScheduleService } from '../../../shared/schedule/schedule.service';
import { AuthService } from '../../../shared/user/auth.service';
import { ScheduleModel } from '../../../shared/schedule/scheduleModels/schedule-model.model';
import { DayOfWeek } from '../../../enums/day-of-week.enum';
import { StudentService } from '../../../shared/student/student.service';
import { StudentScheduleModel } from '../../../shared/schedule/scheduleModels/student-schedule-model.model';

@Component({
  selector: 'app-student-schedule',
  standalone: false,
  templateUrl: './student-schedule.component.html',
  styleUrls: ['../../user-profile-schedule.css']
})
export class StudentScheduleComponent {
  schedule: StudentScheduleModel[] = [];
  groupedSchedule: { [key: string]: StudentScheduleModel[] } = {};
  dayOfWeekEnum = Object.keys(DayOfWeek).filter(v => isNaN(Number(v)));

  constructor(
    private studentService: StudentService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const studentId = +this.authService.getStudentId()!;
    this.studentService.getScheduleByStudentId(studentId).subscribe({
      next: (res) => {
        this.schedule = res;
        this.groupSchedulesByDay();
      },
      error: (err) => {
        console.error('Error loading student schedule:', err);
      }
    });
  }

  groupSchedulesByDay(): void {
    this.groupedSchedule = {};

    for (const day of this.dayOfWeekEnum) {
      this.groupedSchedule[day] = [];
    }

    for (const slot of this.schedule) {
      const dayName = slot.dayOfWeek;
      if (!this.groupedSchedule[dayName]) {
        this.groupedSchedule[dayName] = [];
      }
      this.groupedSchedule[dayName].push(slot);
    }
  }
}
