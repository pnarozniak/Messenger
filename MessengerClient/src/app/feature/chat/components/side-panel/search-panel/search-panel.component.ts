import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatFormField } from '@angular/material/form-field';
import { finalize } from 'rxjs';
import { User } from 'src/app/shared/models/user.model';
import { ChatInList } from '../../../models/chat-in-list.model';
import { SearchApiService } from '../../../services/search-api.service';
import { CHATS_SEARCH_FETCH_SIZE, MORE_PEOPLE_SEARCH_FETCH_SIZE } from 'src/app/app-globals';
import { SearchOption } from '../../../models/search-option.model';

@Component({
  selector: 'app-search-panel',
  templateUrl: './search-panel.component.html',
  styleUrls: ['./search-panel.component.scss']
})
export class SearchPanelComponent implements OnInit {
  @Input() beforeEverySearchCallback!: () => void;
  @Input() afterEverySearchCallback!: (searchPhrase: string) => void;
  @Input() chatsFetchedCallback!: (chats: ChatInList[]) => void;
  @Input() morePeopleFetchedCallback!: (morePeople: User[], error: string | undefined) => void;

  @ViewChild(MatFormField) searchInput!: MatFormField;

  searchOptionEnum: typeof SearchOption = SearchOption;
  @Output() activeOptionChange = new EventEmitter<SearchOption>();
  @Input() activeOption!: SearchOption;

  @Input() isSearching!: boolean;
  @Output() isSearchingChange = new EventEmitter<boolean>();

  phrase: string = '';
  
  constructor(private searchApiService: SearchApiService) { }

  ngOnInit(): void {
    setTimeout(() => this.searchInput.updateOutlineGap(), 100);
  }

  searchForData() {
    if (this.isSearching)
      return;

    this.beforeEverySearchCallback();
    if (!this.phrase && this.activeOption == SearchOption.CHATS) {
      this.afterEverySearchCallback(this.phrase);
      return;
    }
 
    this.setIsSearching(true);
    switch (this.activeOption) {
      case SearchOption.CHATS:
        this.fetchChats();
        break;
      case SearchOption.MORE_PEOPLE:
        this.fetchMorePeople();
        break;
    }
  }

  private fetchChats() : void {
    const { phrase } = this;

    this.searchApiService.getChats(0, CHATS_SEARCH_FETCH_SIZE, phrase)
      .pipe(finalize(() => {
        this.setIsSearching(false);
        this.afterEverySearchCallback(phrase);
      }))
      .subscribe(chats => {
        this.chatsFetchedCallback(chats);
      });
  }

  private fetchMorePeople() : void {
    const { phrase } = this;

    if (!phrase || phrase.length < 3) {
      this.morePeopleFetchedCallback([], 'Search phraze is too short');
      this.setIsSearching(false);
      this.afterEverySearchCallback(phrase);
      return;
    } 
 
    this.searchApiService.getMorePeople(0, MORE_PEOPLE_SEARCH_FETCH_SIZE, phrase)
      .pipe(finalize(() => {
        this.setIsSearching(false);
        this.afterEverySearchCallback(phrase)
      }))
      .subscribe({
        next: (morePeople) => {
          this.morePeopleFetchedCallback(morePeople, undefined);
        },
        error: ({status}) => {
          if (status === 400)
            this.morePeopleFetchedCallback([], 'Search phraze is too short');
        }
      })
  }

  setIsSearching(isSearching: boolean) {
    this.isSearching = isSearching;
    this.isSearchingChange.emit(this.isSearching);
  }

  setSearchOption(option: SearchOption) {
    if (this.isSearching)
      return;

    this.activeOption = option;
    this.activeOptionChange.emit(this.activeOption);
    this.searchForData();
  }

  handleSearchKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter' || !this.phrase) {
      return this.searchForData();
    }
  }
}
