import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifiactionsEvents, NotificationsService } from 'src/app/core/services/notifications.service';
import { ChatInList, LastMessage } from '../../models/chat-in-list.model';
import { Message } from '../../models/message.model';
import { ChatApiService } from '../../services/chat-api.service';
import { ChatStateService } from '../../services/chat-state.service';

@Component({
  selector: 'app-chat-routing-parent',
  templateUrl: './chat-routing-parent.component.html',
  styleUrls: ['./chat-routing-parent.component.scss']
})
export class ChatRoutingParentComponent implements OnInit, OnDestroy {

  constructor(
    private activatedRoute: ActivatedRoute,
    private chatStateService: ChatStateService,
    private chatApiService: ChatApiService,
    private router: Router,
    private notificationsService: NotificationsService) { }

  ngOnInit(): void {
    this.checkRouteAndRedicrect();
    this.notificationsService.startConnection();
    this.notificationsService.addEventListener(
      NotifiactionsEvents.NewMessage, this.onNewMessage
    );
    this.notificationsService.addEventListener(
      NotifiactionsEvents.MessageRemoved, this.onMessageRemoved
    )
  }

  ngOnDestroy(): void {
    this.notificationsService.removeEventListener(
      NotifiactionsEvents.NewMessage, this.onNewMessage
    );
    this.notificationsService.removeEventListener(
      NotifiactionsEvents.MessageRemoved, this.onMessageRemoved
    );
  }

  private checkRouteAndRedicrect(): void {
    const chats = this.activatedRoute.snapshot.data['chats'] as ChatInList[];
    if (chats.length === 0) {
      this.router.navigate(['/chat/new']);
    } else {
      this.router.navigate(['/chat/' + chats[0].id]);
    }
    this.chatStateService.addChats(chats);
  }

  private onNewMessage = (idChat: number, message: Message): void => {
    const lastMessage = new LastMessage(message.id!, message.text, new Date(message.sendDate), false);
    const chatExists = this.chatStateService
      .updateChatLastMessage(idChat, lastMessage);

    if (!chatExists) {
      this.chatApiService.getChatInfo(idChat).subscribe(chatInfo => {
        this.chatStateService.addChats([
          new ChatInList(idChat, chatInfo.name, chatInfo.isPrivate, lastMessage)
        ]);
      });
    }
  }

  private onMessageRemoved = (idChat: number, idMessage: number): void => {
    const currentLastMessage = this.chatStateService.getChatById(idChat)?.lastMessage;

    if (currentLastMessage?.id !== idMessage)
      return;

    const newLastMessage = new LastMessage(idMessage, '', currentLastMessage.sendDate, true);
    this.chatStateService.updateChatLastMessage(idChat, newLastMessage);
  }
}
