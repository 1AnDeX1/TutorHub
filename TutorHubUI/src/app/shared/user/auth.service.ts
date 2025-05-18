import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginModel } from './userModels/login-model.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TOKEN_KEY } from '../constants';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly loginUrl = environment.apiBaseUrl + '/Authentication/login';

  constructor(private http: HttpClient) {}

  login(model: LoginModel): Observable<{ token: string; email: string }> {
    return this.http.post<{ token: string; email: string }>(this.loginUrl, model).pipe(
      tap(response => {
        this.saveToken(response.token);

        const claims = this.getClaims();

        const teacherId = claims?.TeacherId;
        const studentId = claims?.StudentId;

        if (teacherId) {
          localStorage.setItem('TeacherId', teacherId);
        }
        if (studentId) {
          localStorage.setItem('StudentId', studentId);
        }
      })
    );
  }

  saveToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem('TeacherId');
    localStorage.removeItem('StudentId');
  }

  isLoggedIn(): boolean {
    return this.getToken() != null;
  }

  getClaims() {
    const token = this.getToken();
    if (!token) return null;

    return JSON.parse(window.atob(token.split('.')[1]));
  }

  getTeacherId(): number | null {
    const id = localStorage.getItem('TeacherId');
    if (!id) {
      return null;
    }
    const parsed = Number(id);
    return isNaN(parsed) ? null : parsed;
  }


  getStudentId(): number | null {
    const id = localStorage.getItem('StudentId');
    if (!id) {
      return null;
    }
    const parsed = Number(id);
    return isNaN(parsed) ? null : parsed;
  }
}
