import { ScheduleSimpleModel } from "../../schedule/scheduleModels/schedule-simple-model.model";

export class StudentTeacherRequestModel {
    studentId!: number;
    teacherId!: number;
    schedules!: ScheduleSimpleModel[];
}
