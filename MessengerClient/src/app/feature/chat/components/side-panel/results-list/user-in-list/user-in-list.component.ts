import { Component, Input } from '@angular/core';
import { User } from 'src/app/shared/models/user.model';

@Component({
  selector: 'app-user-in-list',
  templateUrl: './user-in-list.component.html',
  styleUrls: ['./user-in-list.component.scss']
})
export class UserInListComponent {
  @Input() user!: User;

  get routerLink() : any[] {
    return ['/chat/new', this.user.id, {'user': JSON.stringify(this.user)}];
  }
}
