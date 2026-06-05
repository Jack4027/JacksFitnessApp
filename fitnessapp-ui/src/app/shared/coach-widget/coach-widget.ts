import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked, NgZone, ChangeDetectorRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MarkdownModule } from 'ngx-markdown';
import { CoachConversation, CoachMessage } from '../../models/coach.models';
import { CoachService } from '../../core/services/coach.service';

@Component({
  selector: 'app-coach-widget',
  standalone: true,
  imports: [FormsModule, MarkdownModule, DatePipe],
  templateUrl: './coach-widget.html',
  styleUrl: './coach-widget.scss'
})
export class CoachWidgetComponent implements OnInit, AfterViewChecked {
  @ViewChild('messagesEnd') messagesEnd!: ElementRef;

  isOpen = false;
  view: 'list' | 'chat' = 'list';

  conversations: CoachConversation[] = [];
  activeConversation: CoachConversation | null = null;
  messages: CoachMessage[] = [];

  messageInput = '';
  isListLoading = false;   // only for conversation list
  isChatLoading = false;   // only for opening a conversation
  isSending = false;
  isCreating = false;
  private shouldScroll = false;
  isEditingTitle = false;
  titleInput = '';

  constructor(private coachService: CoachService, private ngZone: NgZone, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {}

  ngAfterViewChecked(): void {
    if (this.shouldScroll) {
      this.shouldScroll = false;
      this.scrollToBottom();
    }
  }

  toggle(): void {
    this.isOpen = !this.isOpen;
    if (this.isOpen && this.conversations.length === 0 && !this.isListLoading) {
      this.loadConversations();
    }
  }

startEditTitle(): void {
  this.titleInput = this.activeConversation?.title || '';
  this.isEditingTitle = true;
}

saveTitle(): void {
  if (!this.activeConversation || !this.titleInput.trim()) {
    this.isEditingTitle = false;
    return;
  }
  this.coachService.updateConversationTitle(this.activeConversation.id, this.titleInput.trim()).subscribe({
    next: () => {
      this.ngZone.run(() => {
        this.activeConversation!.title = this.titleInput.trim();
        // update in list too
        const convo = this.conversations.find(c => c.id === this.activeConversation!.id);
        if (convo) convo.title = this.titleInput.trim();
        this.isEditingTitle = false;
        this.cdr.detectChanges();
      });
    },
    error: () => {
      this.ngZone.run(() => {
        this.isEditingTitle = false;
        this.cdr.detectChanges();
      });
    }
  });
}

cancelEditTitle(): void {
  this.isEditingTitle = false;
}

loadConversations(): void {
  this.isListLoading = true;
  this.coachService.getConversations().subscribe({
    next: convos => {
      this.conversations = convos;
      this.isListLoading = false;
      this.cdr.detectChanges();
    },
    error: () => {
      this.isListLoading = false;
      this.cdr.detectChanges();
    }
  });
}

openConversation(convo: CoachConversation): void {
  this.isChatLoading = true;
  this.coachService.getConversation(convo.id).subscribe({
    next: full => {
      this.activeConversation = full;
      this.messages = full.messages;
      this.view = 'chat';
      this.isChatLoading = false;
      this.shouldScroll = true;
      this.cdr.detectChanges();
    },
    error: () => {
      this.isChatLoading = false;
      this.cdr.detectChanges();
    }
  });
}

newConversation(): void {
  if (this.isCreating) return;
  this.isCreating = true;

  this.coachService.createConversation().subscribe({
    next: convo => {
      this.conversations.unshift(convo);
      this.activeConversation = { ...convo, messages: [] };
      this.messages = [];
      this.view = 'chat';
      this.isCreating = false;
      this.cdr.detectChanges();
    },
    error: () => {
      this.isCreating = false;
      this.cdr.detectChanges();
    }
  });
}
  sendMessage(): void {
    const content = this.messageInput.trim();
    if (!content || !this.activeConversation || this.isSending) return;

    this.messages.push({
      id: Date.now(),
      content,
      isUserMessage: true,
      createdAt: new Date().toISOString()
    });
    this.messageInput = '';
    this.isSending = true;
    this.shouldScroll = true;

this.coachService.sendMessage(this.activeConversation.id, content).subscribe({
  next: res => {
    this.messages.push({
      id: Date.now() + 1,
      content: res.response,
      isUserMessage: false,
      createdAt: new Date().toISOString()
    });
    this.isSending = false;
    this.shouldScroll = true;
    this.cdr.detectChanges();
  },
  error: () => {
    this.isSending = false;
    this.cdr.detectChanges();
  }
});
  }

  backToList(): void {
    this.view = 'list';
    this.activeConversation = null;
    this.messages = [];
    if (this.conversations.length === 0) {
      this.loadConversations();
    }
  }

  deleteConversation(id: number, event: Event): void {
    event.stopPropagation();
    this.coachService.deleteConversation(id).subscribe({
      next: () => {
        this.conversations = this.conversations.filter(c => c.id !== id);
      }
    });
  }

  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.sendMessage();
    }
  }

  private scrollToBottom(): void {
    this.ngZone.runOutsideAngular(() => {
      try {
        this.messagesEnd.nativeElement.scrollIntoView({ behavior: 'smooth' });
      } catch {}
    });
  }
}