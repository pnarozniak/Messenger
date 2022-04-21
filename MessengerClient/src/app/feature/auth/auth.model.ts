export interface LoginRequestModel {
    email: string;
    plainPassword: string;
}

export interface TokenResponseModel {
    accessToken: string;
    refreshToken: string;
}

export interface RegisterRequestModel {
    firstName: string;
    lastName: string;
    birthDate: string;
    email: string;
    plainPassword: string;
}

export interface EmailVerificationRequestModel {
    email: string;
    token: string;
}