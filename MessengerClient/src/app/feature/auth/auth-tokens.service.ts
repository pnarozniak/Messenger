import { Injectable } from "@angular/core";

@Injectable()
export class AuthTokensService {
    saveAccessToken(accessToken: string) {
        localStorage.setItem("access_token", accessToken);
    }

    saveRefreshToken(refreshToken: string) {
        localStorage.setItem("refresh_token", refreshToken);
    }

    getAccessToken(): string | null{
        return localStorage.getItem("access_token");
    }
    
    getRefreshToken(): string | null{
        return localStorage.getItem("refresh_token");
    }
}