import { Component } from '@angular/core';
import { ScheduleModel } from '../../../shared/schedule/scheduleModels/schedule-model.model';
import { ScheduleService } from '../../../shared/schedule/schedule.service';
import { AuthService } from '../../../shared/user/auth.service';
import { DayOfWeek } from '../../../enums/day-of-week.enum';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-teacher-schedule',
  standalone: false,
  templateUrl: './teacher-schedule.component.html',
  styles: ``
})
export class TeacherScheduleComponent {
  schedule: ScheduleModel[] = [];
  scheduleForm: FormGroup;
  selectedSchedule: ScheduleModel | null = null;
  dayOfWeekEnum = Object.keys(DayOfWeek).filter(v => isNaN(Number(v)));
  teacherId: number | null = null;

  constructor(
    private scheduleService: ScheduleService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.scheduleForm = this.fb.group({
      dayOfWeek: [0, Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      studentId: [null, Validators.required] // ✅ added studentId control
    });
  }

  ngOnInit(): void {
    this.teacherId = this.authService.getTeacherId();

    if (this.teacherId) {
      this.loadSchedule();
    } else {
      console.warn('Teacher ID not found');
    }
  }

  loadSchedule(): void {
    if (!this.teacherId) return;

    this.scheduleService.getScheduleByTeacherId(this.teacherId).subscribe({
      next: (res) => {
        this.schedule = res;
      },
      error: (err) => {
        console.error('Error loading schedule:', err);
      }
    });
  }

  getDayName(dayIndex: number): string {
    return DayOfWeek[dayIndex];
  }

  onSubmit(): void {
    if (!this.teacherId || this.scheduleForm.invalid) return;

    const formValue = this.scheduleForm.value;

    if (this.selectedSchedule) {
      // Update
      const updatedSchedule: ScheduleModel = {
        ...this.selectedSchedule,
        ...formValue
      };

      this.scheduleService.updateSchedule(updatedSchedule).subscribe({
        next: () => {
          this.loadSchedule();
          this.resetForm();
        },
        error: (err) => console.error('Update failed:', err)
      });
    } else {
      // Create
      this.scheduleService.createSchedule(this.teacherId, formValue.studentId, formValue).subscribe({
        next: (res) => {
          this.schedule.push(res);
          this.resetForm();
        },
        error: (err) => console.error('Creation failed:', err)
      });
    }
  }

  onEdit(schedule: ScheduleModel): void {
    this.selectedSchedule = schedule;
    this.scheduleForm.patchValue({
      dayOfWeek: schedule.dayOfWeek,
      startTime: schedule.startTime,
      endTime: schedule.endTime,
      studentId: null // if studentId is present
    });
  }

  onDelete(id: number): void {
    this.scheduleService.deleteSchedule(id).subscribe({
      next: () => {
        this.schedule = this.schedule.filter(s => s.id !== id);
        if (this.selectedSchedule?.id === id) {
          this.resetForm();
        }
      },
      error: (err) => console.error('Delete failed:', err)
    });
  }

  resetForm(): void {
    this.scheduleForm.reset({
      dayOfWeek: 0,
      startTime: '',
      endTime: '',
      studentId: null
    });
    this.selectedSchedule = null;
  }
}
