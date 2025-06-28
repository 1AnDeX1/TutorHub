import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http'
import { environment } from '../../../environments/environment';
import { TeacherModel } from './teacherModels/teacher-model.model';
import { TeacherCreateModel } from './teacherModels/teacher-create-model.model';
import { NgForm } from '@angular/forms';
import { Observable } from 'rxjs';
import { TeacherRatingModel } from './teacherModels/teacher-rating-model.model';
import { TeacherFilter } from './teacherModels/teacher-filter.model';
import { PagedTeacherResult } from './teacherModels/paged-teacher-result.model';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {

  private url = environment.apiBaseUrl + '/Teachers';
  private availableTeachersUrl = environment.apiBaseUrl + '/Teachers/availableTeachers';
  list: TeacherModel[] = [];

  constructor(private http: HttpClient) { }

  getTeachers(): Observable<{ teachers: TeacherModel[], teachersCount: number }> {
    return this.http.get<{ teachers: TeacherModel[], teachersCount: number }>(this.url);
  }

  getAvailableTeachers(filter: TeacherFilter): Observable<PagedTeacherResult> {
    return this.http.post<PagedTeacherResult>(this.availableTeachersUrl, filter);
  }



  getTeacherById(id: number): Observable<TeacherModel> {
    return this.http.get<TeacherModel>(`${this.url}/${id}`);
  }

  putTeacher(id: number, teacher: TeacherCreateModel): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, teacher);
  }

  postTeacher(teacher: TeacherCreateModel): Observable<TeacherModel> {
    return this.http.post<TeacherModel>(this.url, teacher);
  }

  deleteTeacher(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }

  rateTeacher(rating: TeacherRatingModel): Observable<any> {
    return this.http.post(`${this.url}/rate`, rating);
  }

  requestVerification(teacherId: number): Observable<void> {
    return this.http.put<void>(`${this.url}/requestVerification/${teacherId}`, {});
  }


  resetForm(form: NgForm): void {
    form.reset();
  }
}
