import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5066/User/login';  // Base API URL

  constructor(private http: HttpClient) {}

  // Login method (POST)
  login(credentials: any, value: any): Observable<any> {
    const url = `{this.apiUrl}/login`;  // Full URL for login
    return this.http.post(url, credentials);
  }

  // Register method (POST)
  register(user: any): Observable<any> {
    const url = `{this.apiUrl}`;  // Full URL for register
    return this.http.post(url, user);
  }

  // Get user by ID (GET)
  getUserById(userId: string): Observable<any> {
    const url = `{this.apiUrl}/${userId}`;  // Full URL for get user by ID
    return this.http.get(url);
  }
}