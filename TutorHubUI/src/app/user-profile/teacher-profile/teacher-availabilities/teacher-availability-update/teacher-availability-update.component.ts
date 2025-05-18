import { Component, OnInit } from '@angular/core';
import { DayOfWeek } from '../../../../enums/day-of-week.enum';
import { TeacherAvailabilityService } from '../../../../shared/teacher/teacher-availability.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';
import { UpdateAvailabilityRequest } from '../../../../shared/teacher/teacher-availability-models/update-availability-request.model';
import { TeacherAvailabilitiesComponent } from '../teacher-availabilities.component';
import { TeacherAvailabilityModel } from '../../../../shared/teacher/teacher-availability-models/teacher-availability-model.model';

@Component({
  selector: 'app-teacher-availability-update',
  standalone: false,
  templateUrl: './teacher-availability-update.component.html',
  styles: ``
})
export class TeacherAvailabilityUpdateComponent implements OnInit {
  availabilityId!: number;
  DayOfWeek = DayOfWeek;
  dayKeys = Object.keys(DayOfWeek).filter(k => !isNaN(+k)).map(k => +k);

  constructor(
    public availabilityService: TeacherAvailabilityService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(){
    this.availabilityId = Number(this.route.snapshot.paramMap.get('id'));
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.availabilityService.updateAvailability(
        this.availabilityId)
        .subscribe({
          next: () => {
            this.toastr.success('Availability updated successfully!', 'Success');
            this.availabilityService.resetForm(form);
            this.router.navigate(['/profile/availabilities']);
          },
          error: err => {
            this.toastr.error(err.error, "Validation Error");
          }
        });
    }
  }
}
