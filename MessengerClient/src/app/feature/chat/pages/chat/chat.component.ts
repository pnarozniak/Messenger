import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { NotifiactionsEvents, NotificationsService } from 'src/app/core/services/notifications.service';
import { ChatBoxComponent } from '../../components/chat-box/chat-box.component';
import { LastMessage } from '../../models/chat-in-list.model';
import { ChatInfo } from '../../models/chat-info.model';
import { Message } from '../../models/message.model';
import { ChatApiService } from '../../services/chat-api.service';
import { ChatStateService } from '../../services/chat-state.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy {
  idChat!: number;
  messages: Message[] = []
  chatInfo!: ChatInfo;
  REQUESTED_MESSAGES_COUNT = 25;

  @ViewChild(ChatBoxComponent) chatBoxComponent!: ChatBoxComponent;

  constructor(
    private activatedRoute: ActivatedRoute,
    private chatApiService: ChatApiService,
    private chatStateService: ChatStateService,
    private notificationsService: NotificationsService) { }

  ngOnInit(): void {
    this.getRouteData();
    this.notificationsService.addEventListener(
      NotifiactionsEvents.NewMessage, this.onNewMessage
    );
    this.notificationsService.addEventListener(
      NotifiactionsEvents.MessageRemoved, this.onMessageRemoved
    );
  }

  ngOnDestroy(): void {
    this.notificationsService.removeEventListener(
      NotifiactionsEvents.NewMessage, this.onNewMessage
    );
    this.notificationsService.removeEventListener(
      NotifiactionsEvents.MessageRemoved, this.onMessageRemoved
    );
  }

  sendMessage = (text: string): Observable<number> => {
    return this.chatApiService.sendMessage(this.idChat, text)
      .pipe(
        tap((messageId) => {
          this.chatStateService
            .updateChatLastMessage(this.idChat, new LastMessage(messageId, text, new Date(), false));
        }
      ));
  }

  deleteMessage = (idMessage: number) => {
    this.chatApiService.deleteMessage(this.idChat, idMessage).subscribe(() => {
      const message = this.messages.filter(m => m.id === idMessage)[0];
      message.isRemoved = true;
      message.text = '';
      this.chatStateService
        .updateChatLastMessage(this.idChat, new LastMessage(idMessage, message.text, new Date(message.sendDate), true));
    });
  }

  loadMoreMessages = (): Observable<Message[]> => {
    return this.chatApiService
      .getChatMessages(this.idChat, this.messages.length, 25);
  }

  private getRouteData() {
    this.activatedRoute.params.subscribe(params => {
      this.idChat = parseInt(params?.['idChat']);
    });
    this.activatedRoute.data.subscribe((resolversData) => {
      this.messages = resolversData['messages'] as Message[];
      this.chatInfo = resolversData['chatInfo'] as ChatInfo;
   })
  }
  
  private onNewMessage = (idChat: number, message: Message) => {
    if (idChat === this.idChat) {
      this.messages = ([message, ...this.messages]);
      this.chatBoxComponent.scrollToBottom();
    }
  }

  private onMessageRemoved = (idChat: number, idMessage: number) => {
    if (idChat === this.idChat) {
      const message = this.messages.filter(m => m.id === idMessage)?.[0];
      if (message) {
        message.isRemoved = true;
        message.text = '';
      }
    }
  }
}
