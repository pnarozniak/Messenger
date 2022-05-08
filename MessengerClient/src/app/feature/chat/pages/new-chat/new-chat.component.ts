import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, Subject } from 'rxjs';
import { User } from 'src/app/shared/models/user.model';
import { ChatApiService } from '../../services/chat-api.service';
import { SearchApiService } from '../../services/search-api.service';

@Component({
  selector: 'app-new-chat',
  templateUrl: './new-chat.component.html',
  styleUrls: ['./new-chat.component.scss']
})
export class NewChatComponent implements OnInit {
  separatorKeysCodes: number[] = [ENTER, COMMA];
  searchInputValue: string = '';
  toUsers: User[] = [];
  foundUsers: User[] = [];
  
  @ViewChild('searchInput') searchInput!: ElementRef<HTMLInputElement>;

  constructor(
    private searchApiService: SearchApiService,
    private chatApiService: ChatApiService,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    const user = this.route.snapshot.params?.['user'];
    if (user) {
      this.toUsers.push(JSON.parse(user));
    }
  }

  handleSearchInputKeyUp() {
    if (this.searchInputValue.length < 3)
      return;

    this.searchApiService.getMorePeople(0, 10, this.searchInputValue)
      .subscribe(users => {
        this.foundUsers = users.filter(u => !this.toUsers.find(u2 => u2.id === u.id));
      });
  }

  userSelected(user: User): void {
    if (!this.toUsers.find(u => u.id === user.id)) {
      this.toUsers.push(user);
      this.foundUsers = this.foundUsers.filter(u => u.id !== user.id);
    }

    this.searchInput.nativeElement.value = '';
    this.searchInputValue = '';
  }

  removeUser(user: User) {
    this.toUsers = this.toUsers.filter(u => u.id !== user.id);
  }

  createChat = (messageText: string) : Observable<number> => {
    this.chatApiService.createChat(this.toUsers.map(u => u.id), messageText)
      .subscribe({
        next: chatId => {
          this.router.navigate(['/chat', chatId]);        },
        error: err => {
          if (err.status === 409) {
            const existingChatId = err.error;
            this.router.navigate(['/chat', existingChatId]);
          }
        }
      });
    
    return of(-1);
  }
}
