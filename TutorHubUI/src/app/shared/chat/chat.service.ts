import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { ChatMessageModel } from './chatModel/chat-message-model.model';
import { AuthService } from '../user/auth.service';
import { ChatModel } from './chatModel/chat-model.model';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private apiUrl: string = environment.apiBaseUrl;
  private hubConnection: signalR.HubConnection | null = null;
  private messageSubject = new Subject<ChatMessageModel>();

  constructor(private http: HttpClient, private authService: AuthService) {}

  // Get all chats for a teacher
  getChatsForTeacher(teacherId: number): Observable<ChatModel[]> {
    return this.http.get<ChatModel[]>(`${this.apiUrl}/Teachers/chats/${teacherId}`);
  }

  // Get all chats for a student
  getChatsForStudent(studentId: number): Observable<ChatModel[]> {
    return this.http.get<ChatModel[]>(`${this.apiUrl}/Students/chats/${studentId}`);
  }

  // Get previous messages for a chat
  getMessagesForChat(chatId: number): Observable<ChatMessageModel[]> {
    return this.http.get<ChatMessageModel[]>(`${this.apiUrl}/ChatMessages/${chatId}`);
  }

  // Start SignalR connection to join real-time messaging
  startConnection(chatId: string): void {
  const token = this.authService.getToken();
  const userId = this.authService.getUserId();

  if (!token || !userId) {
    console.error('Token or user ID missing!');
    return;
  }

  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(`${environment.apiBaseUrl.replace('/api', '')}/chathub?access_token=${token}`)
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

  this.hubConnection
    .start()
    .then(() => this.joinGroup(chatId, userId))
    .catch(err => console.error('Connection error:', err));

  this.hubConnection.on('ReceiveMessage', (message: ChatMessageModel) => {
    this.messageSubject.next(message);
  });
}

// Join SignalR group
private joinGroup(chatId: string, userId: string): void {
  console.log(`Attempting to join chat group with chatId=${chatId}, userId=${userId}`);

  this.hubConnection
    ?.invoke('JoinChatGroup', chatId, userId)
    .then(() => console.log(`Joined group ${chatId}`))
    .catch(err => console.error('Failed to join group:', err));
}

  // Send a message to the backend hub
  sendMessage(chatId: string, message: string): void {
  const userId = this.authService.getUserId();
  const userName = this.authService.getUserName();

  if (!this.hubConnection) {
    console.error('SignalR connection not established');
    return;
  }

  // Send message to the backend
  this.hubConnection
    ?.invoke('SendMessageToChat', chatId, userId, message)
    .then(() => console.log('Message sent:', { chatId, userId, message, userName }))
    .catch((err) => console.error('Error sending message:', err));
}

  // Subscribe to real-time messages
  onNewMessage(): Observable<ChatMessageModel> {
    return this.messageSubject.asObservable();
  }
}