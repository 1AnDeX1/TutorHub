import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ScheduleModel } from './scheduleModels/schedule-model.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  private readonly baseUrl = environment.apiBaseUrl + '/Schedule';
  
   constructor(private http: HttpClient) {}

  getScheduleByTeacherId(teacherId: number): Observable<ScheduleModel[]> {
    return this.http.get<ScheduleModel[]>(`https://localhost:7094/api/teachers/${teacherId}/schedule`);
  }
// CREATE
  createSchedule(
    teacherId: number,
    studentId: number,
    schedule: { dayOfWeek: number; startTime: string; endTime: string }
  ): Observable<ScheduleModel> {
    return this.http.post<ScheduleModel>(
      `${this.baseUrl}?teacherId=${teacherId}&studentId=${studentId}`,
      schedule
    );
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
