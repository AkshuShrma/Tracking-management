import { Component, OnInit } from '@angular/core';
import { InvitationService } from 'src/app/services/invitation.service';

@Component({
  selector: 'app-invitationers',
  templateUrl: './invitationers.component.html',
  styleUrls: ['./invitationers.component.scss'],
})
export class InvitationersComponent implements OnInit {
  showTable:boolean=false;
  invitations: any;
  invitationsId:string="";
  constructor(private invite: InvitationService) {}

  ngOnInit(): void {
    this.DisplayUsers();
  }

  DisplayUsers() {
    debugger
    this.invite.InvitationComesFrom().subscribe({
      next: (data) => {
        console.log(data);
        this.invitations = data;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  showTableClick(invitationId:any){
   this.showTable=false;
   if(this.invitationsId==invitationId){
    this.showTable=true;
    return;
   }
   setTimeout(()=>{
    this.invitationsId=invitationId;
    this.showTable=false;
   },100)
  }
}
