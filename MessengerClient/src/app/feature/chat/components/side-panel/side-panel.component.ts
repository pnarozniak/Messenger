import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/shared/models/user.model';
import { ChatInList } from '../../models/chat-in-list.model';
import { ChatStateService } from '../../services/chat-state.service';
import { CHATS_INIT_FETCH_SIZE, CHATS_SEARCH_FETCH_SIZE, LOAD_MORE_CHATS_FETCH_SIZE } from 'src/app/app-globals';
import { SearchApiService } from '../../services/search-api.service';
import { SearchOption } from '../../models/search-option.model';

@Component({
  selector: 'app-side-panel',
  templateUrl: './side-panel.component.html',
  styleUrls: ['./side-panel.component.scss']
})
export class SidePanelComponent implements OnInit {
  isSearching: boolean = false;
  activeSearchOption: SearchOption = SearchOption.CHATS;
  lastSearchPhrase: string = '';
  searchError?: string;
  results: { morePeople: User[], chats: ChatInList[] } = {morePeople: [], chats: []}
  allChatsLoaded: boolean = false;

  constructor(
    public chatStateServie: ChatStateService,
    private searchApiService: SearchApiService) { }

  ngOnInit(): void {
    if (this.chatStateServie.chats.length < CHATS_INIT_FETCH_SIZE) {
      this.allChatsLoaded = true;
    }
  }

  beforeEverySearch = () => {
    this.searchError = undefined;
    this.results.morePeople = [];
    this.results.chats = [];
  }

  afterEverySearch = (searchPhrase: string) => {
    this.lastSearchPhrase = searchPhrase;
  }

  onChatsSearched = (chats: ChatInList[]) => {
    if (chats.length < CHATS_SEARCH_FETCH_SIZE) {
      this.allChatsLoaded = true;
    }
    this.results.chats = chats;
  }

  onMorePeopleSearached = (morePeople: User[], error: string | undefined) => {
    this.results.morePeople = morePeople;
    this.searchError = error;
  }

  loadMoreChat = () => {
    this.searchApiService
      .getChats(this.chatStateServie.chats.length, LOAD_MORE_CHATS_FETCH_SIZE, this.lastSearchPhrase)
      .subscribe(chats => {
        if (chats.length < LOAD_MORE_CHATS_FETCH_SIZE) {
          this.allChatsLoaded = true;
        }
        this.chatStateServie.addChats(chats);
      });
  }
}
