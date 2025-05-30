export class ChatMessageModel {
    chatId!: number;
    userId!: string;
    senderName!: string;
    senderRole!: string;
    message!: string;
    createdAt?: string;
}
