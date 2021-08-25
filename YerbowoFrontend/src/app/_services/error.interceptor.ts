import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        
        const authReq = req.clone({
            withCredentials: true // Important for consistent session id in ASP.NET
        });
        
        return next.handle(authReq).pipe(
            catchError(error => {
                if (error instanceof HttpErrorResponse) {
                    if (error.status === 401) {
                        return throwError(error.statusText);
                    }

                    const serverError = error.error.error; 

                    if (serverError && typeof serverError === 'string') {
                        return throwError(serverError);
                    }
                    
                    const serverErrors = error.error.errors;
                    
                    let modalStateErrors = '';

                    if (serverErrors && typeof serverErrors === 'object') {
                        for (const key in serverErrors) {
                            
                            if (serverErrors[key]) {
                                if(serverErrors[key].length > 1) {
                                    for (let i = 0; i < serverErrors[key].length; i++) {
                                        modalStateErrors += serverErrors[key][i];
                                    }
                                } else {
                                    modalStateErrors += serverErrors[key] + '\r\n';
                                }
                            }
                        }
                    }

                    return throwError(modalStateErrors || serverErrors || 'Błąd serwera');
                }
            })
        );
    }
}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};
