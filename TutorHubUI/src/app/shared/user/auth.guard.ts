import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  
  const authService = inject(AuthService);
  const router = inject(Router);
  
  if(authService.isLoggedIn()){
    const claimReq = route.data['claimReq'] as Function;
    if(claimReq){
      const claims = authService.getClaims();
      if(!claimReq(claims)){
        router.navigateByUrl('/forbidden')
        return false;
      }
      return true;
    }
    return true;
  }
  else{
    router.navigateByUrl('/user/login')
    return false
  }
};
