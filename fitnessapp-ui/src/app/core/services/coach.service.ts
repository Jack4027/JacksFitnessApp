import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CoachConversation, CoachMessage, SendMessageResponse } from '../../models/coach.models';
import { API_BASE_URL } from '../config/api.config';


@Injectable({ providedIn: 'root' })
export class CoachService {
  private readonly base = `${API_BASE_URL}/coach`;

  constructor(private http: HttpClient) {}

  getConversations(): Observable<CoachConversation[]> {
    return this.http.get<CoachConversation[]>(`${this.base}/conversations`);
  }

  getConversation(id: number): Observable<CoachConversation> {
    return this.http.get<CoachConversation>(`${this.base}/conversations/${id}`);
  }

  createConversation(): Observable<CoachConversation> {
    return this.http.post<CoachConversation>(`${this.base}/conversations`, {});
  }

  updateConversationTitle(id: number, title: string): Observable<void> {
  return this.http.patch<void>(`${this.base}/conversations/${id}`, { title });
}

  sendMessage(conversationId: number, message: string): Observable<SendMessageResponse> {
    return this.http.post<SendMessageResponse>(
      `${this.base}/conversations/${conversationId}/messages`,
      { message }
    );
  }

  deleteConversation(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/conversations/${id}`);
  }
}