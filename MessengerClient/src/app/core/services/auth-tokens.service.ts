import { Injectable } from "@angular/core";

@Injectable({
    providedIn: "root"
})
export class AuthTokensService{
    saveTokens(accessToken: string, refreshToken: string): void {
        localStorage.setItem("accessToken", accessToken);
        localStorage.setItem("refreshToken", refreshToken);
    }
    
    getTokens(): { accessToken: string | null, refreshToken: string | null } {
        return {
            accessToken: localStorage.getItem("accessToken"),
            refreshToken: localStorage.getItem("refreshToken")
        }
    }

    removeTokens(): void {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
    }
}