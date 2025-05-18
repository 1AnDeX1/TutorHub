import { Subject } from "../../../enums/subject.enum";

export class TeacherCreateModel {
    userName: string = "";
    email: string = "";
    password?: string = "";
    subjects: Subject[] = [];
    hourlyRate: number = 0;
    online: boolean = false;
    offline: boolean = false;
    age: number = 0;
    description: string = "";
  }