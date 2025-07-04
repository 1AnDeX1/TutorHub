import { Injectable } from '@angular/core';
import { StudentCreateModel } from './studentModels/student-create-model.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { ScheduleModel } from '../schedule/scheduleModels/schedule-model.model';
import { StudentScheduleModel } from '../schedule/scheduleModels/student-schedule-model.model';
import { StudentModel } from './studentModels/student-model.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  url: string = environment.apiBaseUrl + '/Students';
  formData: StudentCreateModel = new StudentCreateModel();
  formSubmitted: boolean = false;

  constructor(private http: HttpClient) { }

  getStudents(): Observable<{ students: StudentModel[], studentsCount: number }> {
    return this.http.get<{ students: StudentModel[], studentsCount: number }>(this.url);
  }

  getStudentById(id: number): Observable<StudentCreateModel> {
    return this.http.get<StudentCreateModel>(`${this.url}/${id}`);
  }

  getScheduleByStudentId(studentId: number): Observable<StudentScheduleModel[]> {
    return this.http.get<StudentScheduleModel[]>(`${this.url}/${studentId}/schedule`);
  }

  updateStudent(id: number, student: StudentCreateModel): Observable<any> {
    return this.http.put(`${this.url}/${id}`, student);
  }

  deleteStudent(id: number): Observable<any> {
    return this.http.delete(`${this.url}/${id}`);
  }

  postStudent(): Observable<any> {
    return this.http.post(this.url, this.formData);
  }

  resetForm(form: any) {
    form.resetForm();
    this.formData = new StudentCreateModel();
    this.formSubmitted = false;
  }
}
