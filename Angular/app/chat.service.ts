import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { io, Socket } from 'socket.io-client';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = 'http://localhost:5066/User/login';  // Corrected base URL
  private socket: Socket;
  private messageSubject = new Subject<{ user: string, message: string }>();

  constructor(private http: HttpClient) {
    this.socket = io('http://localhost:5066/user');  // Corrected WebSocket URL
    this.socket.on('ReceiveMessage', (user: string, message: string) => {
      this.messageSubject.next({ user, message });
    });
  }

  // Create a new chat (POST)
  createChat(chat: any): Observable<any> {
    return this.http.post(this.apiUrl, chat);
  }

  // Get chat by ID (GET)
  getChatById(chatId: string): Observable<any> {
    const url = `{this.apiUrl}/{chatId}`;  // Corrected URL formatting
    return this.http.get(url);
  }

  // Update chat (PUT)
  updateChat(chatId: string, updatedChat: any): Observable<any> {
    const url = `{this.apiUrl}/{chatId}`;  // Corrected URL formatting
    return this.http.put(url, updatedChat);
  }

  // Delete chat (DELETE)
  deleteChat(chatId: string): Observable<any> {
    const url = `{this.apiUrl}/{chatId}`;  // Corrected URL formatting
    return this.http.delete(url);
  }

  // Send a real-time message
  sendMessage(user: string, message: string): void {
    this.socket.emit('SendMessage', user, message);
  }

  // Get real-time messages
  getMessages(): Observable<{ user: string, message: string }> {
    return this.messageSubject.asObservable();
  }
}