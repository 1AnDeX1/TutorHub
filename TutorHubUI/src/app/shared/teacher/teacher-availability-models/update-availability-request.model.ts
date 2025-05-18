// update-availability-request.model.ts
export class UpdateAvailabilityRequest {
  teacherId!: number;
  dayOfWeek!: number;
  startTime!: string;
  endTime!: string;
}
