import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { AuthTokensService } from "./auth-tokens.service";

@Injectable()
export class AuthService{
    constructor(private jwtHelperService: JwtHelperService,
        private authTokensService: AuthTokensService) { }

    isAuthenticated() : boolean {
        const accessToken = this.authTokensService.getAccessToken();
        const refreshToken = this.authTokensService.getRefreshToken();
        
        if (accessToken && refreshToken && !this.jwtHelperService.isTokenExpired(accessToken)) {
            return true;
        }

        return false;
    }
}