export interface MessageForDetail {
    messageId: number;
    text: string;
    senderId: number;
    recieverId: number;
    senderName: string;
    recieverName: string;
    dateTimeSent: Date | string | null;
    dateTimeRead: Date | string | null;
    isRead: boolean;
}
