import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService implements HttpInterceptor {

constructor() { }
  intercept(req: import('@angular/common/http').HttpRequest<any>, next: import('@angular/common/http').HttpHandler)
      : import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {

    return next.handle(req).pipe(
      catchError(errorResponse => {
        let errorMessage = '';
        let errorArray = [];
        if (errorResponse instanceof HttpErrorResponse) {
            if (errorResponse.status === 0) {
              return throwError('No Connection');
            }
            if (errorResponse.status === 403) {
              return throwError('Forbidden');
            }

            if (errorResponse.status === 401) {
              return throwError('Unauthorized');
            }

            if (errorResponse.status === 404) {
              return throwError('Not Found');
            }

            if (typeof errorResponse.error.errors === 'object' ) {
              for (const err of Object.keys(errorResponse.error.errors)) {
                  errorArray = errorArray.concat(errorResponse.error.errors[err]);
              }
            } else if (typeof errorResponse.error === 'object' ) {
                for (const err of Object.keys(errorResponse.error)) {
                  errorArray = errorArray.concat(errorResponse.error[err]);
                }

            }  else if (typeof(errorResponse.error)  === 'string') {
                errorMessage = errorResponse.error;
            }
          // tslint:disable-next-line: align
          return throwError(errorMessage || errorArray.join('\n') || errorResponse.statusText || 'ServerError');
          }
        }
      )
    );
  }


}
