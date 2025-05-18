import { DayOfWeek } from "../../../enums/day-of-week.enum";

export class TeacherAvailabilityRequest {
  dayOfWeek!: DayOfWeek;
  startTime!: string;
  endTime!: string;
}
