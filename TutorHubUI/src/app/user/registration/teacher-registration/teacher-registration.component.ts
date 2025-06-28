import { Component } from '@angular/core';
import { Subject } from '../../../enums/subject.enum';
import { TeacherService } from '../../../shared/teacher/teacher.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { TeacherCreateModel } from '../../../shared/teacher/teacherModels/teacher-create-model.model';

@Component({
  selector: 'app-teacher-registration',
  standalone: false,
  templateUrl: './teacher-registration.component.html',
  styleUrl: './../../user.component.css'
})
export class TeacherRegistrationComponent {
  subjectEnum = Subject;
  subjectKeys: number[] = Object.keys(Subject)
    .filter(key => !isNaN(Number(key)))
    .map(key => Number(key));

  formData: TeacherCreateModel = new TeacherCreateModel();
  formSubmitted = false;

  constructor(
    private teacherService: TeacherService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  onSubmit(form: NgForm) {
    this.formSubmitted = true;

    if (form.valid) {
      this.teacherService.postTeacher(this.formData).subscribe({
        next: () => {
          this.resetForm(form);
          this.toastr.success("Created successfully", "Teacher Register");
          this.router.navigate(['/main']);
        },
        error: err => {
        }
      });
    }
  }

  resetForm(form: NgForm) {
    form.resetForm();
    this.formData = new TeacherCreateModel();
    this.formSubmitted = false;
  }

  onSubjectCheckboxChange(event: Event, subjectKey: number): void {
    const checkbox = event.target as HTMLInputElement;
    const index = this.formData.subjects.indexOf(subjectKey);

    if (checkbox.checked && index === -1) {
      this.formData.subjects.push(subjectKey);
    } else if (!checkbox.checked && index !== -1) {
      this.formData.subjects.splice(index, 1);
    }
  }
}
