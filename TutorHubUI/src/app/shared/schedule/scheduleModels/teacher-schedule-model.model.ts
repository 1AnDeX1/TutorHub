export class TeacherScheduleModel {
    id!: number;
    studentTeacherId!: number;
    studentId!: number;
    dayOfWeek!: number;
    startTime!: string;
    endTime!: string;
}
