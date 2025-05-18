import { Component, OnInit } from '@angular/core';
import { TeacherAvailabilityModel } from '../../../shared/teacher/teacher-availability-models/teacher-availability-model.model';
import { TeacherAvailabilityService } from '../../../shared/teacher/teacher-availability.service';
import { DayOfWeek } from '../../../enums/day-of-week.enum';
import { UpdateAvailabilityRequest } from '../../../shared/teacher/teacher-availability-models/update-availability-request.model';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../shared/user/auth.service';

@Component({
  selector: 'app-teacher-availabilities',
  standalone: false,
  templateUrl: './teacher-availabilities.component.html',
  styles: ``
})
export class TeacherAvailabilitiesComponent implements OnInit {
  teacherId: number | null = null;
  dayOfWeekEnum = DayOfWeek;
  constructor(
    private authService: AuthService,
    public availabilityService: TeacherAvailabilityService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.teacherId = this.authService.getTeacherId()!;
    this.availabilityService.getAvailabilitiesByTeacherId(this.teacherId).subscribe({
      next: res => this.availabilityService.list = res,
      error: err => console.error(err)
    });
  }

  getDayName(dayIndex: number): string {
    return DayOfWeek[+dayIndex];
  }

  updateAvailability(selectedAvailability: TeacherAvailabilityModel) {
    this.availabilityService.updateFormData = new UpdateAvailabilityRequest();
    this.availabilityService.updateFormData.teacherId = selectedAvailability.teacherId;
    this.availabilityService.updateFormData.dayOfWeek = selectedAvailability.dayOfWeek;
    this.availabilityService.updateFormData.startTime = selectedAvailability.startTime;
    this.availabilityService.updateFormData.endTime = selectedAvailability.endTime;
  }

  deleteAvailability(id: number) {
    if (confirm('Are you sure you want to delete this availability?')) {
      this.availabilityService.deleteAvailability(id).subscribe({
        next: () => {
          this.toastr.success('Availability deleted successfully!', 'Success');
        },
        error: err => {
          this.toastr.error(err.error, 'Error deleting availability');
        }
      });
    }
  }
}
