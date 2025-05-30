import { Component, OnInit } from '@angular/core';
import { DayOfWeek } from '../../../enums/day-of-week.enum';
import { TeacherAvailabilityService } from '../../../shared/teacher/teacher-availability.service';
import { AuthService } from '../../../shared/user/auth.service';
import { TeacherAvailabilityModel } from '../../../shared/teacher/teacher-availability-models/teacher-availability-model.model';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms'
import { TeacherAvailabilityRequest } from '../../../shared/teacher/teacher-availability-models/teacher-availability-request.model';

@Component({
  selector: 'app-teacher-availability',
  standalone: false,
  templateUrl: './teacher-availabilities.component.html',
  styleUrls: ['./teacher-availabilities.component.css']
})
export class TeacherAvailabilitiesComponent implements OnInit {
  availabilities: TeacherAvailabilityModel[] = [];
  availabilityForm: FormGroup;
  dayOfWeekEnum = Object.keys(DayOfWeek).filter(v => isNaN(Number(v)));
  groupedAvailabilities: { [key: string]: TeacherAvailabilityModel[] } = {};
  clickedSlotId: number | null = null;
  selectedAvailability: TeacherAvailabilityModel | null = null;
  teacherId: number | null = null;
  showForm: boolean = false; // New state for toggling form visibility

  constructor(
    private availabilityService: TeacherAvailabilityService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.availabilityForm = this.fb.group({
      dayOfWeek: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.teacherId = this.authService.getTeacherId();
    if (this.teacherId) {
      this.loadAvailabilities();
    } else {
      console.warn('Teacher ID not found');
    }
  }

  loadAvailabilities(): void {
    if (!this.teacherId) return;

    this.availabilityService.getAvailabilitiesByTeacherId(this.teacherId).subscribe({
      next: (res) => {
        this.availabilities = res;
        this.groupAvailabilitiesByDay();
      },
      error: (err) => console.error('Error loading availabilities:', err)
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

  toggleForm(): void {
    this.showForm = !this.showForm;

    if (!this.showForm) {
      // Reset the form if hiding it
      this.resetForm();
    }
  }

  onSubmit(): void {
    if (!this.teacherId || this.availabilityForm.invalid) return;

    const formValue = this.availabilityForm.value;

    if (this.selectedAvailability) {
      // Update availability
      const payload: TeacherAvailabilityModel = {
        ...this.selectedAvailability,
        ...formValue
      };

      this.availabilityService.updateAvailability(this.selectedAvailability.id, payload).subscribe({
        next: () => {
          this.loadAvailabilities();
          this.resetForm();
          this.showForm = false;
        },
        error: (err) => console.error('Error updating availability:', err)
      });
    } else {
      // Create availability
      const payload: TeacherAvailabilityRequest = {
        dayOfWeek: parseInt(formValue.dayOfWeek, 10),
        startTime: formValue.startTime,
        endTime: formValue.endTime
      };

      this.availabilityService.postAvailability(this.teacherId, payload).subscribe({
        next: () => {
          this.loadAvailabilities();
          this.resetForm();
          this.showForm = false;
        },
        error: (err) => console.error('Error creating availability:', err)
      });
    }
  }

  onEdit(availability: TeacherAvailabilityModel): void {
    this.selectedAvailability = availability;
    this.availabilityForm.patchValue({
      dayOfWeek: availability.dayOfWeek,
      startTime: availability.startTime,
      endTime: availability.endTime
    });
    this.showForm = true; // Show the form when editing
  }

  onSlotClick(slot: TeacherAvailabilityModel): void {
    this.clickedSlotId = this.clickedSlotId === slot.id ? null : slot.id;
  }

  onDelete(id: number): void {
    this.availabilityService.deleteAvailability(id).subscribe({
      next: () => {
        this.availabilities = this.availabilities.filter(a => a.id !== id);
        if (this.selectedAvailability?.id === id) {
          this.resetForm();
          this.showForm = false;
        }
      },
      error: (err) => console.error('Error deleting availability:', err)
    });
  }

  resetForm(): void {
    this.availabilityForm.reset({
      dayOfWeek: '',
      startTime: '',
      endTime: ''
    });
    this.selectedAvailability = null;
  }
}