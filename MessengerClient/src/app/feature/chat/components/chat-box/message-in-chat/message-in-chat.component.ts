import { Component, Input, OnInit } from '@angular/core';
import { Message } from '../../../models/message.model';

@Component({
  selector: 'app-message-in-chat',
  templateUrl: './message-in-chat.component.html',
  styleUrls: ['./message-in-chat.component.scss']
})
export class MessageInChatComponent implements OnInit {
  @Input() message!: Message
  @Input() previousMessage?: Message
  @Input() loggedUserId!: number;
  @Input() deleteMessageCallback?: (message: Message) => void;

  constructor() { }

  ngOnInit(): void {
    
  }
  
  get isOwnMessage(): boolean {
    return this.message.sender.id === this.loggedUserId;
  }

  get isPreviousMessageFromSameUser(): boolean {
    return !!this.previousMessage && this.previousMessage.sender.id === this.message.sender.id;
  }

  deleteMessage(): void {
    this.deleteMessageCallback?.(this.message);
  }
}
