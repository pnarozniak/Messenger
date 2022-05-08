import { Injectable } from "@angular/core";

@Injectable()
export class AuthStateService {
    emailToVerify?: string;
}