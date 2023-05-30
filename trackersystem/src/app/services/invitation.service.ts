import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InvitationService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<any> {
    return this.http.get<any>(`https://localhost:7195/minimalAPI/getAll`);
  }

  invite(username: any): Observable<any> {
    return this.http.get<any>(
      `https://localhost:7195/minimalAPI/invitation/${username}`
    );
  }

  create(data: any): Observable<any> {
    return this.http.post<any>(
      'https://localhost:7195/minimalAPI/createinvitation',
      data
    );
  }

  status(): Observable<any> {
    return this.http.get<any>('');
  }

  action(): Observable<any> {
    return this.http.get<any>('');
  }
}
