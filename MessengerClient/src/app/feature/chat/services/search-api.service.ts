import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { ApiService } from "src/app/core/services/api.service";
import { User } from "src/app/shared/models/user.model";
import { ChatInList } from "../models/chat-in-list.model";

@Injectable()
export class SearchApiService {
    constructor(private apiService: ApiService) {}

    getChats(skipCount: number, takeCount: number, searchPhraze: string = '') : Observable<ChatInList[]> {
        const params = this.buildSearchParams(skipCount, takeCount, searchPhraze);
            
        return this.apiService
            .get<ChatInList[]>('search/chats', params)
            .pipe(map(chats => chats.map(chat => ChatInList.deserialize(chat))));
    }

    getMorePeople(skipCount: number, takeCount: number, searchPhraze: string = '') : Observable<User[]> {
        const params = this.buildSearchParams(skipCount, takeCount, searchPhraze);
            
        return this.apiService.get<User[]>('search/more-people', params);
    }
    
    private buildSearchParams(skipCount: number, takeCount: number, searchPhraze: string) {
        const params = new HttpParams()
                            .set('searchPhraze', searchPhraze)
                            .set('skipCount', skipCount.toString())
                            .set('takeCount', takeCount.toString());

        return params;
    }
}