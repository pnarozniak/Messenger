import { NgModule } from "@angular/core";
import { ChatBoxComponent } from './components/chat-box/chat-box.component';
import { SearchApiService } from "./services/search-api.service";
import { SharedModule } from "src/app/shared/shared.module";
import { NewChatComponent } from './pages/new-chat/new-chat.component';
import { ChatComponent } from "./pages/chat/chat.component";
import { ChatApiService } from "./services/chat-api.service";
import { ChatStateService } from "./services/chat-state.service";
import { SearchPanelComponent } from './components/side-panel/search-panel/search-panel.component';
import { ChatRoutingParentComponent } from './pages/chat-routing-parent/chat-routing-parent.component';
import { ChatsResolver } from "./resolvers/chats.resolver";
import { PickerModule } from '@ctrl/ngx-emoji-mart';
import { MessageInChatComponent } from './components/chat-box/message-in-chat/message-in-chat.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { SidePanelComponent } from './components/side-panel/side-panel.component';
import { ResultsListComponent } from "./components/side-panel/results-list/results-list.component";
import { ChatInListComponent } from "./components/side-panel/results-list/chat-in-list/chat-in-list.component";
import { UserInListComponent } from "./components/side-panel/results-list/user-in-list/user-in-list.component";
import { MessagesResolver } from "./resolvers/messages.resolver";
import { ChatInfoResolver } from "./resolvers/chat-info.resolver";
import { NewChatWithUserGuard } from "./guards/new-chat-with-user.guard";

@NgModule({
  declarations: [
    ResultsListComponent,
    ChatBoxComponent,
    NewChatComponent,
    ChatComponent,
    SearchPanelComponent,
    ChatRoutingParentComponent,
    MessageInChatComponent,
    ChatInListComponent,
    UserInListComponent,
    SidePanelComponent,
  ],
  imports: [
    SharedModule,
    PickerModule,
    MatAutocompleteModule
  ],
  providers: [
    SearchApiService,
    ChatApiService,
    ChatStateService,
    ChatsResolver,
    MessagesResolver,
    ChatInfoResolver,
    NewChatWithUserGuard
  ]
})
export class ChatModule { }