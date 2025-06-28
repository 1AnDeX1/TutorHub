import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../shared/admin/admin.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-verifications',
  standalone: false,
  templateUrl: './verifications.component.html',
  styles: ``
})
export class VerificationsComponent implements OnInit {
  pendingTeachers: any[] = [];
  totalTeachers = 0;
  currentPage = 1;
  pageSize = 20;
  searchName?: string | null;
  isLoading = false;

  constructor(
    private adminService: AdminService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadPendingRequests();
  }

  loadPendingRequests(): void {
    this.isLoading = true;

    const nameParam: string | null = this.searchName?.trim() === '' ? null : this.searchName?.trim() || null;

    this.adminService.getPendingVerificationRequests(nameParam, this.currentPage, this.pageSize)
      .subscribe({
        next: (res) => {
          this.pendingTeachers = res.teachers;
          this.totalTeachers = res.teachersCount;
          this.isLoading = false;
        },
        error: () => {
          this.toastr.error('Failed to load verification requests');
          this.isLoading = false;
        }
      });
  }


  approve(teacherId: number): void {
    this.adminService.approveVerification(teacherId).subscribe({
      next: () => {
        this.toastr.success('Verification approved');
        this.loadPendingRequests();
      },
      error: () => this.toastr.error('Failed to approve verification')
    });
  }

  reject(teacherId: number): void {
    this.adminService.rejectVerification(teacherId).subscribe({
      next: () => {
        this.toastr.success('Verification rejected');
        this.loadPendingRequests();
      },
      error: () => this.toastr.error('Failed to reject verification')
    });
  }
}
