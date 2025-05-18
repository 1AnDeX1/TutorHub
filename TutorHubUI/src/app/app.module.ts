import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainPageComponent } from './main-page/main-page.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TeacherDetailsComponent } from './main-page/teacher-details/teacher-details.component';
import { StudentProfileDetailsComponent } from './user-profile/student-profile/student-profile-details/student-profile-details.component';
import { TeacherProfileDetailsComponent } from './user-profile/teacher-profile/teacher-profile-details/teacher-profile-details.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { TeacherProfileComponent } from './user-profile/teacher-profile/teacher-profile.component';
import { TeacherAvailabilitiesComponent } from './user-profile/teacher-profile/teacher-availabilities/teacher-availabilities.component';
import { TeacherAvailabilityAddComponent } from './user-profile/teacher-profile/teacher-availabilities/teacher-availability-add/teacher-availability-add.component';
import { TeacherAvailabilityUpdateComponent } from './user-profile/teacher-profile/teacher-availabilities/teacher-availability-update/teacher-availability-update.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { UserComponent } from './user/user.component';
import { TeacherRegistrationComponent } from './user/registration/teacher-registration/teacher-registration.component';
import { StudentRegistrationComponent } from './user/registration/student-registration/student-registration.component';
import { LoginComponent } from './user/login/login.component';
import { AuthInterceptor } from './shared/user/auth.interceptor';
import { AdminComponent } from './admin/admin.component';
import { StudentScheduleComponent } from './user-profile/student-profile/student-schedule/student-schedule.component';
import { StudentProfileComponent } from './user-profile/student-profile/student-profile.component';
import { ForbiddenComponent } from './user/forbidden/forbidden.component';
import { HideIfClaimsNotMetDirective } from './shared/directives/hide-if-claims-not-met.directive';
import { TeacherStudentComponent } from './user-profile/teacher-profile/teacher-student/teacher-student.component';
import { StudentTeacherComponent } from './user-profile/student-profile/student-teacher/student-teacher.component';
import { TeacherScheduleComponent } from './user-profile/teacher-profile/teacher-schedule/teacher-schedule.component';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    TeacherDetailsComponent,
    StudentProfileDetailsComponent,
    TeacherProfileDetailsComponent,
    TeacherProfileComponent,
    TeacherAvailabilitiesComponent,
    TeacherAvailabilityAddComponent,
    TeacherAvailabilityUpdateComponent,
    DashboardComponent,
    UserComponent,
    TeacherRegistrationComponent,
    StudentRegistrationComponent,
    LoginComponent,
    AdminComponent,
    StudentScheduleComponent,
    StudentProfileComponent,
    ForbiddenComponent,
    HideIfClaimsNotMetDirective,
    TeacherScheduleComponent,
    TeacherStudentComponent,
    StudentTeacherComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    ToastrModule.forRoot()
  ],
  providers: [{
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
