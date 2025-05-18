import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TeacherAvailabilityModel } from './teacher-availability-models/teacher-availability-model.model';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { TeacherAvailabilityRequest } from './teacher-availability-models/teacher-availability-request.model';
import { NgForm } from '@angular/forms';
import { UpdateAvailabilityRequest } from './teacher-availability-models/update-availability-request.model';

@Injectable({
  providedIn: 'root'
})
export class TeacherAvailabilityService {

  url: string = environment.apiBaseUrl + '/TeacherAvailabilities';
  list: TeacherAvailabilityModel[] = [];
  formData: TeacherAvailabilityRequest = new TeacherAvailabilityRequest();
  updateFormData: UpdateAvailabilityRequest = new UpdateAvailabilityRequest();
  formSubmitted: boolean = false;
  constructor(private http: HttpClient) { }

  getAvailabilitiesByTeacherId(teacherId: number): Observable<TeacherAvailabilityModel[]> {
    return this.http.get<TeacherAvailabilityModel[]>(`${this.url}/${teacherId}`);
  }

  postAvailability(teacherId: number): Observable<void> {
    return this.http.post<void>(`${this.url}/${teacherId}/add`, this.formData);
  }

  updateAvailability(id: number): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, this.updateFormData);
  }

  deleteAvailability(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }

  resetForm(form: NgForm){
    form.reset();
    this.formData = new TeacherAvailabilityRequest();
    this.updateFormData = new UpdateAvailabilityRequest();
    this.formSubmitted = false;
  }
}
