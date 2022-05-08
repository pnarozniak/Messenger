import { Component, Input } from '@angular/core';
import { User } from 'src/app/shared/models/user.model';
import { ChatInList } from '../../../models/chat-in-list.model';
import { SearchOption } from '../../../models/search-option.model';
import { ChatStateService } from '../../../services/chat-state.service';

@Component({
  selector: 'app-results-list',
  templateUrl: './results-list.component.html',
  styleUrls: ['./results-list.component.scss']
})
export class ResultsListComponent {
  @Input() searchedMorePeople: User[] = [];
  @Input() searchedChats: ChatInList[] = [];
  @Input() isSearching!: boolean;
  @Input() activeSearchOption!: SearchOption;
  @Input() searchError?: string;
  @Input() lastSearchPhrase: string = '';
  @Input() allChatsAreLoaded: boolean = false;
  @Input() loadMoreChatsCallback?: () => void;

  constructor(public chatStateService: ChatStateService) { }

  get noSearchResults() {
    return this.searchedChats.length === 0 && this.searchedMorePeople.length === 0
  }

  get showChatsFromState() {
    return this.noSearchResults && this.activeSearchOption == SearchOption.CHATS
     && !this.searchError && this.lastSearchPhrase === ''
  }
  
  get showChatSections() {
    return this.lastSearchPhrase !== '' && this.searchedChats.length > 0
  }
  
  get showErrorSection() {
    const conditionForChats = 
      this.activeSearchOption == SearchOption.CHATS ? this.lastSearchPhrase !== '' : true;
    
    return this.searchedChats.length <= 0 && this.searchedMorePeople.length <= 0 
    && !this.isSearching && conditionForChats;
  }

  loadMoreChats() {
    this.loadMoreChatsCallback?.();
  }
}
