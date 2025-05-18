import { Component } from '@angular/core';
import { claimReq } from './shared/utils/claimReq-utils';
import { AuthService } from './shared/user/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styles: []
})
export class AppComponent {

  constructor(public authService: AuthService){

  }

  claimReq = claimReq
  title = 'TutorHubUI';
}
