import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

// Function to check and add Authorization header if required
function addAuthHeaderIfRequired(req: HttpRequest<any>, token: string | null): HttpRequest<any> {
  const shouldAddAuth = req.urlWithParams.includes('addAuth=true');

  if (shouldAddAuth && token) {
    return req.clone({
      setHeaders: {
        authorization: token,
      },
    });
  }

  // Return the original request if no modification is needed
  return req;
}

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const cookieService = inject(CookieService);
  const token = cookieService.get('Authorization');

  // Use the helper function to potentially modify the request
  const modifiedReq = addAuthHeaderIfRequired(req, token);

  return next(modifiedReq);
};
