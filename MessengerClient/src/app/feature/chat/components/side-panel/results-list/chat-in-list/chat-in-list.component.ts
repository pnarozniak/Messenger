import { Component, Input } from '@angular/core';
import { ChatInList } from 'src/app/feature/chat/models/chat-in-list.model';

@Component({
  selector: 'app-chat-in-list',
  templateUrl: './chat-in-list.component.html',
  styleUrls: ['./chat-in-list.component.scss']
})
export class ChatInListComponent {
  @Input() chat!: ChatInList;
}
