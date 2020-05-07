import { Component, OnInit } from '@angular/core';
import { MessagesService } from 'src/app/services/Repositories/messages.service';
import { MessageForDetail } from 'src/app/Models/MessageForDetail';
import { MatTabChangeEvent } from '@angular/material/tabs';
import {MatDialog, MatDialogConfig, MatDialogRef} from '@angular/material/dialog';
import { MessageModalComponent } from '../messageModal/messageModal.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: MessageForDetail[] = [];

  constructor(private messageService: MessagesService, private dialog: MatDialog) { }

  ngOnInit() {
    this.getUnreadMessages();
  }

  getUnreadMessages() {
    this.messageService.getUnreadMessages().subscribe(response => {
      this.messages = response;
    });
  }
  getInboxMessages() {
    this.messageService.getInboxMessages().subscribe(response => {
      this.messages = response;
    });
  }
  getOutboxMessages() {
    this.messageService.getOutboxMessages().subscribe(response => {
      this.messages = response;
    });
  }

  tabchanged($event: MatTabChangeEvent) {
    switch ($event.index) {
      case 0: this.getUnreadMessages(); break;
      case 1: this.getInboxMessages(); break;
      case 2: this.getOutboxMessages(); break;
      default: this.getUnreadMessages(); break;
    }
  }

  onChatOpen($event) {
    const  config = new MatDialogConfig();
    config.minWidth = '380px';
    config.width = '600px';
    config.data = $event;
    console.log(config);
    this.dialog.open(MessageModalComponent, config);
  }
}
