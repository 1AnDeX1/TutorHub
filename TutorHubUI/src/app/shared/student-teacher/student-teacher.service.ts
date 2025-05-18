import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { StudentTeacherRequestModel } from './studentTeacherModels/student-teacher-request-model.model';
import { Observable } from 'rxjs';
import { StudentModel } from '../student/studentModels/student-model.model';
import { TeacherModel } from '../teacher/teacherModels/teacher-model.model';

@Injectable({
  providedIn: 'root'
})
export class StudentTeacherService {
  
  private readonly baseUrl = environment.apiBaseUrl + '/student-teacher';

  constructor(private http: HttpClient) { }

  requestStudentToTeacher(request: StudentTeacherRequestModel): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/request`, request);
  }

  getStudentsOfTeacher(teacherId: number, status: number) {
    return this.http.get<StudentModel[]>(
      `${this.baseUrl}/students/${teacherId}?status=${status}`
    );
  }

  getTeachersOfStudent(studentId: number, status: number): Observable<TeacherModel[]> {
    return this.http.get<TeacherModel[]>(
      `${this.baseUrl}/teachers/${studentId}?status=${status}`
    );
  }

  confirmStudent(teacherId: number, studentId: number): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/confirm?teacherId=${teacherId}&studentId=${studentId}`,
      {}
    );
  }

  rejectStudent(teacherId: number, studentId: number): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/reject?teacherId=${teacherId}&studentId=${studentId}`,
      {}
    );
  }

  unsubscribe(teacherId: number, studentId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/unsubscribe?teacherId=${teacherId}&studentId=${studentId}`
    );
  }
}
