import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { 
    path: 'auth', 
    loadChildren: () => import('./feature/auth/auth-routing.module').then(m => m.AuthRoutingModule) 
  },
  { 
    path: 'chat', 
    loadChildren: () => import('./feature/chat/chat-routing.module').then(m => m.ChatRoutingModule),
    canActivate: [AuthGuard]
  },
  { path: '', redirectTo: '/chat', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
