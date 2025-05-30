import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpErrorResponse,
  HttpEvent,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ApiErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error instanceof HttpErrorResponse) {
          const errorMessage = error.error || 'An unexpected error occurred';
          const errorStatus = error.status;

          if (errorStatus === 400) {
            this.toastr.error(errorMessage, 'Error');
          } else if (errorStatus === 500) {
            this.toastr.error(
              'The server encountered an error. Please try again later.',
              'Server Error'
            );
          } else {
            this.toastr.error(errorMessage, 'Error');
          }
        }

        return throwError(() => error);
      })
    );
  }
}