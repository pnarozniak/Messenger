import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { ChatInList } from "../models/chat-in-list.model";
import { SearchApiService } from "../services/search-api.service";
import { CHATS_INIT_FETCH_SIZE } from "src/app/app-globals";

@Injectable()
export class ChatsResolver implements Resolve<ChatInList[]> {

    constructor(private searchApiService: SearchApiService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): 
    ChatInList[] | Observable<ChatInList[]> | Promise<ChatInList[]> {
        return this.searchApiService.getChats(0, CHATS_INIT_FETCH_SIZE);
    }
}