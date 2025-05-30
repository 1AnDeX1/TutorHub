import { Component } from '@angular/core';
import { ChatModel } from '../../../shared/chat/chatModel/chat-model.model';
import { ChatService } from '../../../shared/chat/chat.service';
import { AuthService } from '../../../shared/user/auth.service';

@Component({
  selector: 'app-student-chats',
  standalone: false,
  templateUrl: './student-chats.component.html',
  styleUrl: '../../user-chat.css'
})
export class StudentChatsComponent {
  chats: ChatModel[] = [];
  studentId: number | null = null; // To store the logged-in student's UserId
  loading: boolean = true;
  error: string | null = null;

  constructor(
    private chatService: ChatService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.studentId = this.authService.getStudentId();

    if (!this.studentId) {
      this.error = 'Student ID not found. Please log in.';
      this.loading = false;
      return;
    }

    this.chatService.getChatsForStudent(Number(this.studentId)).subscribe({
      next: (chats) => {
        this.chats = chats;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching student chats:', err);
        this.error = 'Failed to load chats. Please try again.';
        this.loading = false;
      },
    });
  }
}
