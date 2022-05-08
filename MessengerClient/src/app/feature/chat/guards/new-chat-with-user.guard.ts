import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { catchError, Observable, of, Subject } from 'rxjs';
import { User } from 'src/app/shared/models/user.model';
import { ChatApiService } from '../services/chat-api.service';

@Injectable()
export class NewChatWithUserGuard implements CanActivate {
  constructor(
    private router: Router,
    private chatApiService: ChatApiService) {}

  canActivate(snapshot: ActivatedRouteSnapshot): Observable<boolean> {
    const idUser = snapshot.paramMap.get('idUser');
    if (!idUser) return this.fallbackRoute();
    
    const stringifiedUser = snapshot.paramMap.get('user');
    if (!stringifiedUser) return this.fallbackRoute();

    const user: User = JSON.parse(stringifiedUser);
    if (!user || user?.id?.toString() != idUser) return this.fallbackRoute();

    const subject = new Subject<boolean>();
    this.chatApiService.getChatIdWithUser(user.id)
      .subscribe((chatId: number | null) => {
          if (chatId) {
            this.router.navigate(['/chat', chatId]);
            subject.next(false);
          } else {
            subject.next(true);
          }
      });
    
    return subject;
  }

  fallbackRoute() : Observable<boolean> {
    this.router.navigate(['/chat/new']);
    return of(true);
  }
}