import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr"
import { AuthTokensService } from "./auth-tokens.service";
import { SIGNAL_R_URL } from "src/app/app-globals";

@Injectable({
    providedIn: "root"
})
export class NotificationsService {
    private hubConnection?: signalR.HubConnection

    constructor(private authTokensService: AuthTokensService) {}

    public startConnection = () => {
        const connOptions: signalR.IHttpConnectionOptions = {
            accessTokenFactory: () => {
                return `${this.authTokensService.getTokens().accessToken!}`;
            }
          };

        this.hubConnection = new signalR.HubConnectionBuilder()
                                .withUrl(`${SIGNAL_R_URL}/notifications`, connOptions)
                                .build();
        
        this.hubConnection
          .start()
          .then(() => console.log('Connection started'))
          .catch(err => console.log('Error while starting connection: ' + err))
    }
    
    public addEventListener = (event: NotifiactionsEvents, callback: (...data: any) => void) => {
        if (this.hubConnection) {
            this.hubConnection.on(event, callback);
        }
    }

    public removeEventListener = (event: NotifiactionsEvents, callbackToRemove: (...data: any) => void) => {
        if (this.hubConnection) {
            this.hubConnection.off(event, callbackToRemove);
        }
    }
}

export enum NotifiactionsEvents {
    NewMessage = "NewMessage",
    MessageRemoved = "MessageRemoved"
}