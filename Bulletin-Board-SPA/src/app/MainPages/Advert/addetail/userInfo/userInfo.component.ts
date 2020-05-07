import { Component, OnInit, Input } from '@angular/core';
import { UserService } from 'src/app/services/Repositories/user.service';
import { UserForDetail } from 'src/app/Models/UserForDetail';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { MessageModalComponent } from 'src/app/MainPages/Messages/messageModal/messageModal.component';

@Component({
  selector: 'app-userInfo',
  templateUrl: './userInfo.component.html',
  styleUrls: ['./userInfo.component.css']
})
export class UserInfoComponent implements OnInit {
  @Input() userId: number;
  @Input() detailedMode: boolean;
  user: UserForDetail;
  newMessage: string;
  constructor(private userService: UserService, private dialog: MatDialog) { }

  ngOnInit() {
    this.userService.getById(this.userId).subscribe(response => {
      this.user = response;
    });
  }

  sendMessage() {

  }

  cancel() {
    this.newMessage = '';
  }

  onChatOpen($event) {
    const  config = new MatDialogConfig();
    config.minWidth = '380px';
    config.width = '600px';
    config.data = $event;
    this.dialog.open(MessageModalComponent, config);

    
  }


  onMessageClick() {
    const chatWithId = this.userId;
    const chatWithName = this.user.userName;
    console.log(chatWithName, chatWithId);
    this.onChatOpen({id: chatWithId, name: chatWithName});
  }

}
