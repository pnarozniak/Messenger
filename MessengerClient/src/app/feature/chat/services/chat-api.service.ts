import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable, catchError, of, pipe } from "rxjs";
import { ApiService } from "src/app/core/services/api.service";
import { ChatInfo } from "../models/chat-info.model";
import { Message } from "../models/message.model";

@Injectable()
export class ChatApiService {

    constructor(private apiService: ApiService) {}

    getChatMessages(idChat: number, skipCount: number, takeCount: number) : Observable<Message[]> {
        const params = new HttpParams()
                            .set('skipCount', skipCount.toString())
                            .set('takeCount', takeCount.toString());
        
        return this.apiService
                        .get<Message[]>(`chats/${idChat}/messages`, params)
                        .pipe(
                            map(m => m.map(m => Message.deserialize(m)))
                        );
    }

    getChatInfo(idChat: number) : Observable<ChatInfo>{
        return this.apiService
                        .get<ChatInfo>(`chats/${idChat}`, undefined)
                        .pipe(
                            map(info => ChatInfo.deserialize(info))
                        );    
    }

    createChat(membersIds: number[], initialMessageText: string) : Observable<number> {
        const body = {
            membersIds: membersIds,
            initialMessageText: initialMessageText
        };

        return this.apiService.post<any, number>(`chats`, body);
    }

    sendMessage(idChat: number, text: string) : Observable<number> {
        return this.apiService
                        .post<any, number>(`chats/${idChat}/messages`, { text: text });
    }

    deleteMessage(idChat: number, idMessage: number) : Observable<any> {
        return this.apiService.delete(`chats/${idChat}/messages/${idMessage}`);
    }

    getChatIdWithUser(userId: number) : Observable<number | null> {
        return this.apiService.get<number>(`chats/with-user/${userId}`, undefined)
                                .pipe(catchError(() => {
                                    return of(null);
                                }));
    }
}   