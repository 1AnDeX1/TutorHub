import { Component } from '@angular/core';
import { ScheduleModel } from '../../../shared/schedule/scheduleModels/schedule-model.model';
import { ScheduleService } from '../../../shared/schedule/schedule.service';
import { AuthService } from '../../../shared/user/auth.service';
import { DayOfWeek } from '../../../enums/day-of-week.enum';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ScheduleSimpleModel } from '../../../shared/schedule/scheduleModels/schedule-simple-model.model';
import { TeacherScheduleModel } from '../../../shared/schedule/scheduleModels/teacher-schedule-model.model';

@Component({
  selector: 'app-teacher-schedule',
  standalone: false,
  templateUrl: './teacher-schedule.component.html',
  styleUrl: '../../user-profile-schedule.css'
})
export class TeacherScheduleComponent {
  schedule: TeacherScheduleModel[] = [];
  scheduleForm: FormGroup;
  selectedSchedule: ScheduleModel | null = null;
  dayOfWeekEnum = Object.keys(DayOfWeek).filter(v => isNaN(Number(v)));
  teacherId: number | null = null;
  groupedSchedule: { [key: string]: TeacherScheduleModel[] } = {};
  clickedSlotId: number | null = null;
  showForm: boolean = false; // State for toggling form visibility

  constructor(
    private scheduleService: ScheduleService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.scheduleForm = this.fb.group({
      dayOfWeek: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      studentId: [null, Validators.required]
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
        this.groupSchedulesByDay();
      },
      error: (err) => {
        console.error('Error loading schedule:', err);
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


  toggleForm(): void {
    this.showForm = !this.showForm;

    if (!this.showForm) {
      // Reset the form if hiding it
      this.resetForm();
    }
  }

  onSubmit(): void {
    if (!this.teacherId || this.scheduleForm.invalid) return;

    const formValue = this.scheduleForm.value;

    if (this.selectedSchedule) {
      // Update schedule
      const updatedSchedule: ScheduleModel = {
        ...this.selectedSchedule,
        ...formValue
      };

      this.scheduleService.updateSchedule(updatedSchedule).subscribe({
        next: () => {
          this.loadSchedule();
          this.resetForm();
          this.showForm = false; // Hide form after update
        },
        error: (err) => console.error('Update failed:', err)
      });
    } else {
      // Create schedule
      const scheduleCreateModel = {
        teacherId: this.teacherId,
        studentId: formValue.studentId,
        dayOfWeek: parseInt(formValue.dayOfWeek, 10),
        startTime: formValue.startTime,
        endTime: formValue.endTime
      };
      this.scheduleService.createSchedule(scheduleCreateModel).subscribe({
        next: () => {
          this.loadSchedule();
          this.resetForm();
          this.showForm = false; // Hide form after creation
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
      studentId: schedule.studentTeacherId
    });
    this.showForm = true; // Show form for editing
  }

  onSlotClick(slot: ScheduleModel): void {
    this.clickedSlotId = this.clickedSlotId === slot.id ? null : slot.id;
  }

  onDelete(id: number): void {
    this.scheduleService.deleteSchedule(id).subscribe({
      next: () => {
        this.schedule = this.schedule.filter(s => s.id !== id);
        if (this.selectedSchedule?.id === id) {
          this.resetForm();
        }
        this.loadSchedule();
      },
      error: (err) => console.error('Delete failed:', err)
    });
  }

  resetForm(): void {
    this.scheduleForm.reset({
      dayOfWeek: '',
      startTime: '',
      endTime: '',
      studentId: null
    });
    this.selectedSchedule = null;
  }
}
