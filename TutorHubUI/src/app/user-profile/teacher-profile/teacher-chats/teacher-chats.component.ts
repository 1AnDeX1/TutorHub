import { Component } from '@angular/core';
import { ChatService } from '../../../shared/chat/chat.service';
import { ChatModel } from '../../../shared/chat/chatModel/chat-model.model';
import { AuthService } from '../../../shared/user/auth.service';

@Component({
  selector: 'app-teacher-chat',
  standalone: false,
  templateUrl: './teacher-chats.component.html',
  styleUrl: '../../user-chat.css'
})
export class TeacherChatsComponent {
  chats: ChatModel[] = [];
  teacherId: number | null = null; // To store the logged-in teacher's UserId
  loading: boolean = true;
  error: string | null = null;

  constructor(
    private chatService: ChatService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.teacherId = this.authService.getTeacherId();

    if (!this.teacherId) {
      this.error = 'Teacher ID not found. Please log in.';
      this.loading = false;
      return;
    }

    this.chatService.getChatsForTeacher(Number(this.teacherId)).subscribe({
      next: (chats) => {
        this.chats = chats;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching teacher chats:', err);
        this.error = 'Failed to load chats. Please try again.';
        this.loading = false;
      },
    });
  }
}
