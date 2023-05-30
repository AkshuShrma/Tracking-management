import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http:HttpClient) { }

  login(data:any):Observable<any>{
    return this.http.post<any>("https://localhost:7195/minimalAPI/login",data);
  }

  register(data:any):Observable<any>{
    return this.http.post<any>("https://localhost:7195/minimalAPI/register",data);
  }
  
}
