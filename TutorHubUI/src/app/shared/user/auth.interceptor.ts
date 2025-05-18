import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService,
    private router:Router,
    private toastr:ToastrService
  ) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.authService.isLoggedIn()) {
      const authReq = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + this.authService.getToken())
      });
      return next.handle(authReq).pipe(
        tap({
          error:(err:any)=>{
            if(err.status == 401){
              this.authService.logout()
              setTimeout(()=>{
                this.toastr.info("Please login again", "Session Expired")
              }, 1500);
              this.router.navigateByUrl('/user/login')
            }
            else if (err.status == 403){
              this.toastr.error("You are not authorized")
            }
          }
        })
      );
    }
    return next.handle(req);
  }
}
