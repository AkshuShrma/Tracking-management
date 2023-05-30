import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  constructor(private http: HttpClient) {}

  get() {
    return this.http.get<any>('https://localhost:7195/minimalAPI/Books');
  }

  create(payload: any) {
    return this.http.post<any>(
      'https://localhost:7195/minimalAPI/newBooks',
      payload
    );
  }

  update(payload: any) {
    return this.http.put<any>(
      `https://localhost:7195/minimalAPI/updateBooks`,
      payload
    );
  }

  delete(bookId: number) {
    return this.http.delete(
      `https://localhost:7195/minimalAPI/deleteBooks/${bookId}`
    );
  }
}
