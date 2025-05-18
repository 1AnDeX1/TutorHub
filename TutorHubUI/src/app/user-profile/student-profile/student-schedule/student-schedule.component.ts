import { Component } from '@angular/core';
import { ScheduleService } from '../../../shared/schedule/schedule.service';
import { AuthService } from '../../../shared/user/auth.service';
import { ScheduleModel } from '../../../shared/schedule/scheduleModels/schedule-model.model';
import { DayOfWeek } from '../../../enums/day-of-week.enum';
import { StudentService } from '../../../shared/student/student.service';

@Component({
  selector: 'app-student-schedule',
  standalone: false,
  templateUrl: './student-schedule.component.html',
  styles: ``
})
export class StudentScheduleComponent {
  schedule: ScheduleModel[] = [];
  dayOfWeekEnum = DayOfWeek;

  constructor(
    private studentService: StudentService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const studentId = +this.authService.getStudentId()!;
    this.studentService.getScheduleByStudentId(studentId).subscribe({
      next: (res) => {
        this.schedule = res;
      },
      error: (err) => {
        console.error('Error loading student schedule:', err);
      }
    });
  }

  getDayName(dayIndex: number): string {
    return DayOfWeek[dayIndex];
  }
}
