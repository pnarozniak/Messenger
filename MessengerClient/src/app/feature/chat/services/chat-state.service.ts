import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { ChatInList, LastMessage } from "../models/chat-in-list.model";

@Injectable()
export class ChatStateService {
    private _chats$ = new BehaviorSubject<ChatInList[]>([]);

    get chats() : ChatInList[] {
        return this._chats$.value;
    }

    get chats$() : Observable<ChatInList[]> {
        return this._chats$.asObservable();
    }
    
    clearChats() {
        if (this.chats.length !== 0)
            this._chats$.next([]);
    }

    getChatById(idChat: number) : ChatInList | undefined{
        return this.chats.find(c => c.id == idChat);
    };

    updateChatLastMessage(idChat: number, lastMessage: LastMessage) : boolean{
        const chat = this.getChatById(idChat);
        if (!chat)
            return false;

        const updatedChat = new ChatInList(chat.id, chat.name, chat.isPrivate, lastMessage);
        this.updateChat(updatedChat);
        return true;
    }
    
    addChats(chatsToAdd: ChatInList[]) {
        let chatsCopy = [...this.chats];
        for (let chat of chatsToAdd) {
            if (chatsCopy.some(c => c.id === chat.id))
                continue;
                
            chatsCopy = this.addChatSilent(chatsCopy, chat);
        }

        this._chats$.next([...chatsCopy]);
    }

    private updateChat(updatedChat: ChatInList) {
        let currentChats = [...this.chats];

        const index = currentChats.findIndex(c => c.id === updatedChat.id);
        if (index === -1)
            return;

        currentChats = [
            ...currentChats.slice(0, index),
            ...currentChats.slice(index + 1)
        ]

        this._chats$.next([
            ...this.addChatSilent(currentChats, updatedChat)
        ]);
    }

    private addChatSilent(chats: ChatInList[], chatToAdd: ChatInList) : ChatInList[]{
        for (let [index, chat] of chats.entries()) {
            if (chatToAdd.lastMessage.sendDate > chat.lastMessage.sendDate) {
                return [
                    ...chats.slice(0, index),
                    chatToAdd,
                    ...chats.slice(index)
                ];
            }
        }

        return [...chats, chatToAdd];
    }
}