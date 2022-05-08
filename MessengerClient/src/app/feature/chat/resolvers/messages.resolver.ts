import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Message } from "../models/message.model";
import { ChatApiService } from "../services/chat-api.service";
import { MESSAGES_INIT_FETCH_SIZE } from "src/app/app-globals";

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {

    constructor(private chatApiService: ChatApiService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): 
    Message[] | Observable<Message[]> | Promise<Message[]> {
        const idChat = route.params?.['idChat'];
        return this.chatApiService.getChatMessages(idChat, 0, MESSAGES_INIT_FETCH_SIZE);
    }
}