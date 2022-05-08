import { HttpClient, HttpErrorResponse, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { API_URL } from "src/app/app-globals";

@Injectable({
  providedIn: "root"
})
export class ApiService {
    constructor(private http: HttpClient) {}

    post<T, R>(url: string, data: T): Observable<R> {
        const reqUrl: string = API_URL + '/' + url;

        return this.http
          .post<R>(reqUrl, data)
          .pipe(catchError(this.handleError));
    }

    get<R>(url: string, params: HttpParams | undefined): Observable<R> {
        const reqUrl: string = API_URL + '/' + url;

        return this.http
          .get<R>(reqUrl, {params: params ?? {}})
          .pipe(catchError(this.handleError));
    }

    delete(url: string): Observable<any> {
        const reqUrl: string = API_URL + '/' + url;

        return this.http
          .delete(reqUrl)
          .pipe(catchError(this.handleError));
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