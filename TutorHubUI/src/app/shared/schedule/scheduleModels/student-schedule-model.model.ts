export class StudentScheduleModel {
    id!: number;
    studentTeacherId!: number;
    teacherId!: number;
    dayOfWeek!: number;
    startTime!: string;
    endTime!: string;
}
