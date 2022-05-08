import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { AuthTokensService } from "./auth-tokens.service";

@Injectable({
    providedIn: "root"
})
export class AuthService {
    constructor(
        private authTokensService: AuthTokensService,
        private jwtHelperService: JwtHelperService) {}

    getUserId(): number {
        const { accessToken } = this.authTokensService.getTokens();
        var decodedToken = this.jwtHelperService.decodeToken(accessToken!);
        return parseInt(
            decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
        )
    }
}