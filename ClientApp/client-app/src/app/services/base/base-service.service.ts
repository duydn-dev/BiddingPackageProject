import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ApiConfig } from '../../confis/api-config';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    'Access-Control-Allow-Origin': '*'
  })
};

@Injectable({
  providedIn: 'root'
})


export class BaseService {

  constructor(private _http: HttpClient) { }
  get(url: string, params?: any): Observable<any> {
    return this._http.get(`${ApiConfig.apiUrl}/${url}${params ? `/${params}` : ''}`, httpOptions);
  }
  getWithQuery(url: string, paramsName: string, params: string): Observable<any> {
    return this._http.get(`${ApiConfig.apiUrl}/${url}?${paramsName}=${params}`, httpOptions);
  }
  post(url: string, body: any): Observable<any> {
    return this._http.post(`${ApiConfig.apiUrl}/${url}`,body, httpOptions);
  }
  put(url: string, params: any, body: any) : Observable<any>{
    return this._http.put(`${ApiConfig.apiUrl}/${url}/${params}`,body, httpOptions);
  }
  delete(url: string, params: any) : Observable<any>{
    return this._http.delete(`${ApiConfig.apiUrl}/${url}/${params}`, httpOptions);
  }
  deleteWithQuery(url: string,paramsName: string, params: string) : Observable<any>{
    return this._http.delete(`${ApiConfig.apiUrl}/${url}?${paramsName}=${params}`, httpOptions);
  }
  uploadFile(url, formData){
    return this._http.post(`${ApiConfig.apiUrl}/${url}`, formData);
  }
}
