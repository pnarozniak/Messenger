import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { ChatInfo } from "../models/chat-info.model";
import { ChatApiService } from "../services/chat-api.service";

@Injectable()
export class ChatInfoResolver implements Resolve<ChatInfo> {

    constructor(private chatApiService: ChatApiService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): 
    ChatInfo | Observable<ChatInfo> | Promise<ChatInfo> {
        const idChat = route.params?.['idChat'];
        return this.chatApiService.getChatInfo(idChat);
    }
}