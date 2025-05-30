import { Subject } from "../../../enums/subject.enum";
import { VerificationStatus } from "../../../enums/verification-status.enum";

export interface TeacherFilter {
  subjects: Subject[] | null;
  minHourlyRate: number | null;
  maxHourlyRate: number | null;
  online: boolean | null;
  offline: boolean | null;
  minAge: number | null;
  maxAge: number | null;
  verificationStatus: VerificationStatus | null;
  page: number;
  pageSize: number;
}

