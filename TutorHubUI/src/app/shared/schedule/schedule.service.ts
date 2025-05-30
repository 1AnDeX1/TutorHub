import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ScheduleModel } from './scheduleModels/schedule-model.model';
import { Observable } from 'rxjs';
import { ScheduleSimpleModel } from './scheduleModels/schedule-simple-model.model';
import { ScheduleCreateModel } from './scheduleModels/schedule-create-model.model';
import { TeacherScheduleModel } from './scheduleModels/teacher-schedule-model.model';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  private readonly baseUrl = environment.apiBaseUrl + '/Schedule';
  
   constructor(private http: HttpClient) {}

  getScheduleByTeacherId(teacherId: number): Observable<TeacherScheduleModel[]> {
    return this.http.get<TeacherScheduleModel[]>(`https://localhost:7094/api/teachers/${teacherId}/schedule`);
  }
  
  createSchedule(scheduleCreateModel: ScheduleCreateModel): Observable<ScheduleCreateModel> {
    return this.http.post<ScheduleCreateModel>(this.baseUrl, scheduleCreateModel);
  }

  // UPDATE
  updateSchedule(schedule: ScheduleModel): Observable<ScheduleModel> {
    return this.http.put<ScheduleModel>(this.baseUrl, schedule);
  }

  // DELETE
  deleteSchedule(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}?id=${id}`);
  }

  // GET by ID
  getScheduleById(id: number): Observable<ScheduleModel> {
    return this.http.get<ScheduleModel>(`${this.baseUrl}/${id}`);
  }

  // GET all schedules (optional, for admin maybe)
  getAllSchedules(): Observable<ScheduleModel[]> {
    return this.http.get<ScheduleModel[]>(this.baseUrl);
  }
}
