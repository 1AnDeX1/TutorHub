import { Subject } from "../../../enums/subject.enum";
import { VerificationStatus } from "../../../enums/verification-status.enum";

export class TeacherModel {
    id: number = 0;
    userName: string = "";
    email: string = "";
    subjects: Subject[] = [];
    hourlyRate: number = 0;
    rating: number = 0;
    online: boolean = false;
    offline: boolean = false;
    age: number = 0;
    description: string = "";
    verificationStatus!: VerificationStatus;
}