import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { NewChatWithUserGuard } from "./guards/new-chat-with-user.guard";
import { ChatRoutingParentComponent } from "./pages/chat-routing-parent/chat-routing-parent.component";
import { ChatComponent } from "./pages/chat/chat.component";
import { NewChatComponent } from "./pages/new-chat/new-chat.component";
import { ChatInfoResolver } from "./resolvers/chat-info.resolver";
import { ChatsResolver } from "./resolvers/chats.resolver";
import { MessagesResolver } from "./resolvers/messages.resolver";

const routes: Routes = [
  {
    path: "",
    component: ChatRoutingParentComponent,
    resolve: { 
      chats: ChatsResolver
    },
    children: [
        {
          path: "new",
          component: NewChatComponent,
        },
        {
          path: "new/:idUser",
          component: NewChatComponent,
          canActivate: [ NewChatWithUserGuard ]
        },
        {
            path: ":idChat",
            resolve: { 
              messages: MessagesResolver,
              chatInfo: ChatInfoResolver
            },
            runGuardsAndResolvers: 'paramsOrQueryParamsChange',
            component: ChatComponent,
        },

    ]
  } 
];;

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule {}