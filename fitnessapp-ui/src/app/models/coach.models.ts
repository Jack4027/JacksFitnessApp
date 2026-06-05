export interface CoachConversation {
  id: number;
  title: string;
  createdAt?: string;
  updatedAt: string;
  messages: CoachMessage[];
}

export interface CoachMessage {
  id: number;
  content: string;
  isUserMessage: boolean;
  createdAt: string;
}

export interface SendMessageResponse {
  response: string;
  conversationId: number;
}