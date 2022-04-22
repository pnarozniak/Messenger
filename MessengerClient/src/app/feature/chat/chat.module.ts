import { NgModule } from "@angular/core";
import { ChatComponent } from './pages/chat/chat.component';
import { ChatsListComponent } from './components/chats-list/chats-list.component';
import { ChatBoxComponent } from './components/chat-box/chat-box.component';
import { ChatOptionsComponent } from './components/chat-options/chat-options.component';

@NgModule({
  declarations: [
    ChatComponent,
    ChatsListComponent,
    ChatBoxComponent,
    ChatOptionsComponent,
  ]
})
export class ChatModule { }