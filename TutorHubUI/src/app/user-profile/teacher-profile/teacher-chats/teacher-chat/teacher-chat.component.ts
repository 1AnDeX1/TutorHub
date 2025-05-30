import { Component, ElementRef, ViewChild } from '@angular/core';
import { ChatMessageModel } from '../../../../shared/chat/chatModel/chat-message-model.model';
import { ActivatedRoute } from '@angular/router';
import { ChatService } from '../../../../shared/chat/chat.service';
import { AuthService } from '../../../../shared/user/auth.service';

@Component({
  selector: 'app-teacher-chat',
  standalone: false,
  templateUrl: './teacher-chat.component.html',
  styleUrl: '../../../user-chat.css'
})
export class TeacherChatComponent {
  chatId: string | null = null;
  userId: string | null = null;
  userName: string | null = null;
  messages: ChatMessageModel[] = [];
  newMessage: string = '';
  loading: boolean = true;
  error: string | null = null;

  @ViewChild('chatLog') private chatLogContainer!: ElementRef;

  constructor(
    private route: ActivatedRoute,
    private chatService: ChatService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.chatId = this.route.snapshot.paramMap.get('id');
    this.userId = this.authService.getUserId();
    this.userName = this.authService.getUserName();

    if (!this.chatId || !this.userId) {
      this.error = 'Invalid chat ID or user.';
      this.loading = false;
      return;
    }

    this.fetchMessages();

    this.chatService.startConnection(this.chatId);

    this.chatService.onNewMessage().subscribe(message => {
      if (String(message.chatId) === this.chatId) {
        this.fetchMessages();
      }
    });
  }

  fetchMessages(): void {
    this.chatService.getMessagesForChat(Number(this.chatId)).subscribe({
      next: (messages) => {
        this.messages = messages;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading messages:', err);
        this.error = 'Failed to load messages.';
        this.loading = false;
      },
    });
  }

  sendMessage(): void {
    if (!this.newMessage.trim()) return;

    this.chatService.sendMessage(this.chatId!, this.newMessage);
    this.newMessage = '';
  }

  autoResize(event: Event) {
    const textarea = event.target as HTMLTextAreaElement;
    textarea.style.height = 'auto'; // reset height
    textarea.style.height = textarea.scrollHeight + 'px'; // set to scrollHeight
  }
  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.chatLogContainer.nativeElement.scrollTop = this.chatLogContainer.nativeElement.scrollHeight;
    } catch(err) { }
  }
}
