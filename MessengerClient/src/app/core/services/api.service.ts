import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ApiService {
    private readonly api_base_url: string = 'https://localhost:7009/api/';

    constructor(private http: HttpClient) {}

    post<T, R>(url: string, data: T): Observable<R> {
        const reqUrl: string = this.api_base_url + url;
        console.log("post to" + reqUrl);
        return this.http.post<R>(reqUrl, data).pipe(
            catchError(this.handleError)
        );
    }

    private handleError(error: HttpErrorResponse) : Observable<never> {
        if (error.status === 0)
          console.error('An error occurred:', error.error);
        else
          console.error(
            `Backend returned code ${error.status}, body was: `, error.error);

        return throwError(() => error);
      }
}