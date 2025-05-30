import { TeacherModel } from "./teacher-model.model";

export interface PagedTeacherResult {
  teachers: TeacherModel[];
  totalPages: number;
  currentPage: number;
}
