import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TeacherModel } from '../teacher/teacherModels/teacher-model.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
private readonly apiUrl = environment.apiBaseUrl + '/Admin'; // Replace with your actual API base URL

  constructor(private http: HttpClient) {}

  getPendingVerificationRequests(
    name: string | null,
    page: number = 1,
    pageSize: number = 20
  ): Observable<{ teachers: TeacherModel[], teachersCount: number }> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (name !== null && name.trim() !== '') {
      params = params.set('name', name.trim());
    }

    return this.http.get<{ teachers: TeacherModel[], teachersCount: number }>(
      `${this.apiUrl}/pendingVerificationRequests`,
      { params }
    );
  }


  approveVerification(teacherId: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/approveVerification/${teacherId}`, {});
  }

  rejectVerification(teacherId: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/rejectVerification/${teacherId}`, {});
  }
}
