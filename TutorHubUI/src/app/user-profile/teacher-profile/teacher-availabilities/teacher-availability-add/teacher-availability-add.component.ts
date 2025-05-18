import { Component } from '@angular/core';
import { TeacherAvailabilityRequest } from '../../../../shared/teacher/teacher-availability-models/teacher-availability-request.model';
import { DayOfWeek } from '../../../../enums/day-of-week.enum';
import { TeacherAvailabilityService } from '../../../../shared/teacher/teacher-availability.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { TeacherAvailabilityModel } from '../../../../shared/teacher/teacher-availability-models/teacher-availability-model.model';
import { AuthService } from '../../../../shared/user/auth.service';

@Component({
  selector: 'app-teacher-availability-add',
  standalone: false,
  templateUrl: './teacher-availability-add.component.html',
  styles: ``
})
export class TeacherAvailabilityAddComponent {
  DayOfWeek = DayOfWeek;
  dayKeys = Object.keys(DayOfWeek)
    .filter(k => !isNaN(+k))
    .map(k => +k);

  constructor(
    private authService: AuthService,
    public availabilityService: TeacherAvailabilityService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  onSubmit(form: NgForm) {
    if (!form.valid) return;

    this.availabilityService.postAvailability(this.authService.getTeacherId()!)
      .subscribe({
        next: () => {
          this.toastr.success('Availability added successfully!', 'Success');
          this.availabilityService.resetForm(form);
          this.router.navigate(['/profile/availabilities']);
        },
        error: err => {
          this.toastr.error(err.error, "Validation Error");
        }
      });
  }
}
