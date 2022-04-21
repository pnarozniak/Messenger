import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiService } from "src/app/core/services/api.service";
import { EmailVerificationRequestModel, LoginRequestModel, RegisterRequestModel, TokenResponseModel } from "./auth.model";

@Injectable()
export class AuthApiService {
    constructor(private apiService: ApiService) {}

    login(loginModel: LoginRequestModel) : Observable<TokenResponseModel>{
        return this.apiService.post<LoginRequestModel, TokenResponseModel>
            ("auth/login", loginModel);
    }

    register(registerModel: RegisterRequestModel) : Observable<TokenResponseModel>{
        return this.apiService.post<RegisterRequestModel, TokenResponseModel>
            ("auth/register", registerModel);
    }

    verifyEmail(verificationModel: EmailVerificationRequestModel) : Observable<any>{
        return this.apiService.post<EmailVerificationRequestModel, any>
            ("auth/verify-email", verificationModel);
    }
}