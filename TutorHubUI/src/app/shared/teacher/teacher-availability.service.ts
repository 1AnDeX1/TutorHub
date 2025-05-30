import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { TeacherAvailabilityModel } from './teacher-availability-models/teacher-availability-model.model';
import { TeacherAvailabilityRequest } from './teacher-availability-models/teacher-availability-request.model';
import { UpdateAvailabilityRequest } from './teacher-availability-models/update-availability-request.model';

@Injectable({
  providedIn: 'root'
})
export class TeacherAvailabilityService {
  private url: string = environment.apiBaseUrl + '/TeacherAvailabilities';

  constructor(private http: HttpClient) {}

  getAvailabilitiesByTeacherId(teacherId: number): Observable<TeacherAvailabilityModel[]> {
    return this.http.get<TeacherAvailabilityModel[]>(`${this.url}/${teacherId}`);
  }

  // Create a new availability
  postAvailability(teacherId: number, payload: TeacherAvailabilityRequest): Observable<void> {
    return this.http.post<void>(`${this.url}/${teacherId}/add`, payload);
  }

  // Update an existing availability
  updateAvailability(id: number, payload: UpdateAvailabilityRequest): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, payload);
  }

  // Delete an availability by ID
  deleteAvailability(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}