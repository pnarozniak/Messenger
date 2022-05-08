import { Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { AlertsService } from 'src/app/core/services/alerts.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { Message } from '../../models/message.model';

@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrls: ['./chat-box.component.scss']
})
export class ChatBoxComponent implements OnInit, OnChanges {
  @Input() messages: Message[] = [];
  @Output() messagesChange = new EventEmitter<Message[]>();
  @Input() messageSendCallback?: (text: string) => Observable<number>;
  @Input() loadMoreMessagesCallback?: () => Observable<Message[]>;
  @Input() messageDeleteCallback?: (idMessage: number) => void;
  @Input() loadMoreMessagesCount: number = 0;
  
  @ViewChild('messagesList') private messagesListRef?: ElementRef;

  loggedUserId!: number;
  messageTextInput: string = '';
  messageFiles: File[] = [];
  allMessagesLoaded: boolean = false;
  MAX_MESSAGE_FILES_COUNT = 5;

  constructor(
    private authService: AuthService,
    private alertsService: AlertsService) { }

  ngOnInit(): void {
    this.loggedUserId = this.authService.getUserId();
    this.allMessagesLoaded = false;
    this.messageFiles = [];
    this.scrollToBottom();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['messages']) {
      this.scrollToBottom();
    }
  }

  setMessages(messages: Message[]) {
    this.messages = messages;
    this.messagesChange.emit(this.messages);
  }

  sendMessage() {
    if (this.messageFiles.length > this.MAX_MESSAGE_FILES_COUNT) {
      this.alertsService.showError(`You can send up to ${this.MAX_MESSAGE_FILES_COUNT} files at once`);
      return;
    }

    const newMessage = new Message( 
      undefined, 
      this.messageTextInput,
      false,
      new Date(),
      { id: this.loggedUserId, firstName: '', lastName: '' }
    );

    this.setMessages([newMessage, ...this.messages]);
    this.scrollToBottom();

    this.messageSendCallback?.(this.messageTextInput)
      .subscribe((messageId) => {
        newMessage.id = messageId;
        const filtered = this.messages.filter(m => m.id && m.sendDate != newMessage.sendDate);
        filtered.unshift(newMessage);
        this.setMessages(filtered);
      });

    this.messageTextInput = '';
    this.messageFiles = [];
  }

  loadMoreMessages() {
    this.loadMoreMessagesCallback?.().subscribe(messages => {
      this.setMessages([...this.messages, ...messages]);
      this.allMessagesLoaded = messages.length < this.loadMoreMessagesCount;
    });
  }

  deleteMessage = (message: Message): void => {
    this.messageDeleteCallback?.(message.id!);
  }

  scrollToBottom() {
    setTimeout(()=>{    
      if (!this.messagesListRef) return;     

      const lastMessageRef = this.messagesListRef.nativeElement.lastElementChild;
      lastMessageRef?.scrollIntoView();
    }, 1);
  }

  addEmoji(event: any) {
    this.messageTextInput += event.emoji.native;
  }

  addFile(event: any) {
    const validFileTypes = ['jpeg', 'png', 'gif', 'jpg', 'svg', 'pdf', 
      'doc', 'docx', 'xls', 'xlsx', 'ppt', 'pptx', 'txt', 
      'csv', 'zip', 'rar', 'cs', 'java', 'c', 'cpp', 
      'c#', 'html', 'css', 'js', 'php', 'sql', 'xml', 'json'];
    
    for (const file of event.target.files) {
      const fileType = file?.name?.split('.')?.pop();
      if (fileType == null || !validFileTypes.includes(fileType)) {
        this.alertsService.showError('Invalid file type');
        continue;
      }

      this.messageFiles.push(file);
    }
  }

  removeFile(file: File) {
    this.messageFiles = this.messageFiles.filter(f => f !== file);
  }
}