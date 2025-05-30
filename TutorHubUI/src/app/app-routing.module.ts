import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { TeacherProfileComponent } from './user-profile/teacher-profile/teacher-profile.component';
import { TeacherAvailabilitiesComponent } from './user-profile/teacher-profile/teacher-availabilities/teacher-availabilities.component';
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
import { TeacherChatsComponent } from './user-profile/teacher-profile/teacher-chats/teacher-chats.component';
import { TeacherChatComponent } from './user-profile/teacher-profile/teacher-chats/teacher-chat/teacher-chat.component';
import { StudentChatsComponent } from './user-profile/student-profile/student-chats/student-chats.component';
import { StudentChatComponent } from './user-profile/student-profile/student-chats/student-chat/student-chat.component';
import { AdminComponent } from './admin/admin.component';
import { TeacherListComponent } from './admin/teacher-list/teacher-list.component';
import { StudentListComponent } from './admin/student-list/student-list.component';

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
      { path: 'chats', component: StudentChatsComponent },
      { path: 'chat/:id', component: StudentChatComponent },
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
      { path: 'availabilities', component: TeacherAvailabilitiesComponent },
      { path: 'schedule', component: TeacherScheduleComponent},
      { path: 'students', component: TeacherStudentComponent},
      { path: 'chats', component: TeacherChatsComponent },  
      { path: 'chat/:id', component: TeacherChatComponent },
      { path: '', redirectTo: 'create', pathMatch: 'full' }
      ]
  },
  {
    path: 'admin',
    component: AdminComponent,
    children: [
      { path: 'teacher-list', component: TeacherListComponent },
      { path: 'student-list', component: StudentListComponent },
    ],
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
