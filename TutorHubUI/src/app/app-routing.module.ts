import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { TeacherProfileComponent } from './user-profile/teacher-profile/teacher-profile.component';
import { TeacherAvailabilitiesComponent } from './user-profile/teacher-profile/teacher-availabilities/teacher-availabilities.component';
import { TeacherAvailabilityAddComponent } from './user-profile/teacher-profile/teacher-availabilities/teacher-availability-add/teacher-availability-add.component';
import { TeacherAvailabilityUpdateComponent } from './user-profile/teacher-profile/teacher-availabilities/teacher-availability-update/teacher-availability-update.component';
import { UserComponent } from './user/user.component';
import { TeacherRegistrationComponent } from './user/registration/teacher-registration/teacher-registration.component';
import { StudentRegistrationComponent } from './user/registration/student-registration/student-registration.component';
import { LoginComponent } from './user/login/login.component';
import { authGuard } from './shared/user/auth.guard';
import { StudentScheduleComponent } from './user-profile/student-profile/student-schedule/student-schedule.component';
import { StudentProfileComponent } from './user-profile/student-profile/student-profile.component';
import { ForbiddenComponent } from './user/forbidden/forbidden.component';
import { claimReq } from './shared/utils/claimReq-utils';
import { TeacherScheduleComponent } from './user-profile/teacher-profile/teacher-schedule/teacher-schedule.component';
import { TeacherDetailsComponent } from './main-page/teacher-details/teacher-details.component';
import { TeacherStudentComponent } from './user-profile/teacher-profile/teacher-student/teacher-student.component';
import { StudentTeacherComponent } from './user-profile/student-profile/student-teacher/student-teacher.component';
import { StudentProfileDetailsComponent } from './user-profile/student-profile/student-profile-details/student-profile-details.component';
import { TeacherProfileDetailsComponent } from './user-profile/teacher-profile/teacher-profile-details/teacher-profile-details.component';

const routes: Routes = [
  { path: 'main', component: MainPageComponent },
  { path: 'teachers/:id', component: TeacherDetailsComponent },
  {
    path: 'student-profile',
    component: StudentProfileComponent,
    canActivate: [authGuard],
    canActivateChild: [authGuard],
    data: {claimReq: claimReq.studentOnly},
    children: [
      { path: 'profile', component: StudentProfileDetailsComponent,},
      { path: 'schedule', component: StudentScheduleComponent,},
      { path: 'teachers', component: StudentTeacherComponent,},
      { path: '', redirectTo: 'create', pathMatch: 'full' }
      ]
  },
  {
    path: 'teacher-profile',
    component: TeacherProfileComponent,
    canActivate: [authGuard],
    canActivateChild: [authGuard],
    data: {claimReq: claimReq.teacherOnly},
    children: [
      {path: 'profile', component: TeacherProfileDetailsComponent },
      { 
        path: 'availabilities',
        component: TeacherAvailabilitiesComponent,
        children: [
          { path: 'add', component: TeacherAvailabilityAddComponent },
          { path: 'update/:id', component: TeacherAvailabilityUpdateComponent }
        ]
      },
      { 
        path: 'schedule',
        component: TeacherScheduleComponent,
        children: [
          { path: 'add', component: TeacherAvailabilityAddComponent },
          { path: 'update/:id', component: TeacherAvailabilityUpdateComponent }
        ]
      },
      { path: 'students', component: TeacherStudentComponent,},
      { path: '', redirectTo: 'create', pathMatch: 'full' }
      ]
  },
  { path: 'user', component: UserComponent,
    children:[
      { path: 'login', component: LoginComponent },
      { path: 'teacher-registration', component: TeacherRegistrationComponent },
      { path: 'student-registration', component: StudentRegistrationComponent }
    ]
  },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: '', redirectTo: '/main', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
