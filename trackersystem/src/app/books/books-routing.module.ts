import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from '../components/home/home.component';
import { AddbooksComponent } from '../components/addbooks/addbooks.component';
import { EditbooksComponent } from '../components/editbooks/editbooks.component';
import { InvitationComponent } from '../components/invitation/invitation.component';
import { ConfirmationComponent } from '../components/confirmation/confirmation.component';
import { InvitationersComponent } from '../components/invitationers/invitationers.component';

const routes: Routes = [
  {
    path:'invitationers',
    component:InvitationersComponent
  },
  {
    path:'confirmation/:reciverId/:status',
    component:ConfirmationComponent,
  },
  {
    path: 'invitation',
    component: InvitationComponent,
  },
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'edit/:bookId',
    component: EditbooksComponent,
  },
  {
    path: 'add',
    component: AddbooksComponent,
  },
  {
    path: 'home',
    component: HomeComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BooksRoutingModule { }
