import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http'
import { environment } from '../../../environments/environment';
import { TeacherModel } from './teacherModels/teacher-model.model';
import { TeacherCreateModel } from './teacherModels/teacher-create-model.model';
import { NgForm } from '@angular/forms';
import { Observable } from 'rxjs';
import { TeacherRatingModel } from './teacherModels/teacher-rating-model.model';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {

  private url = environment.apiBaseUrl + '/Teachers';
  list: TeacherModel[] = [];

  constructor(private http: HttpClient) { }

  getTeachers(): Observable<{ teachers: TeacherModel[], teachersCount: number }> {
    return this.http.get<{ teachers: TeacherModel[], teachersCount: number }>(this.url);
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

  // shared/student-teacher/student-teacher.service.ts
  rateTeacher(rating: TeacherRatingModel): Observable<any> {
    return this.http.post(`${this.url}/rate`, rating);
  }


  resetForm(form: NgForm): void {
    form.reset();
  }
}
