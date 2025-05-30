import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { StudentService } from '../../shared/student/student.service';
import { StudentModel } from '../../shared/student/studentModels/student-model.model';

@Component({
  selector: 'app-student-list',
  standalone: false,
  templateUrl: './student-list.component.html',
  styles: ``
})
export class StudentListComponent {
  isLoading = false; // For spinner
  students: StudentModel[] = []; // To store the list of students
  totalStudents: number = 0; // To track the total count of students

  constructor(
    private studentService: StudentService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadStudents(); // Fetch students when the component loads
  }

  // Fetch the list of students from the service
  loadStudents(): void {
    this.isLoading = true;

    this.studentService.getStudents().subscribe(
      (data) => {
        this.students = data.students;
        this.totalStudents = data.studentsCount;
        this.isLoading = false;
      },
      (error) => {
        this.toastr.error('Failed to load students');
        this.isLoading = false;
      }
    );
  }

  // Delete a student by ID
  deleteStudent(id: number): void {
    if (confirm('Are you sure you want to delete this student?')) {
      this.studentService.deleteStudent(id).subscribe(
        () => {
          this.toastr.success('Student deleted successfully');
          this.loadStudents(); // Reload the list after deletion
        },
        (error) => {
          this.toastr.error('Failed to delete student');
        }
      );
    }
  }
}
