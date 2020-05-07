import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MessageForDetail } from 'src/app/Models/MessageForDetail';


@Component({
  selector: 'app-messagesTable',
  templateUrl: './messagesTable.component.html',
  styleUrls: ['./messagesTable.component.css']
})
export class MessagesTableComponent implements OnInit {

  // @Input() dataColumns: string[];
  dataColumns = ['message', 'name', 'datesent', 'dateread', 'actions'];
  @Input() box: string;
  @Input() dataSet: MessageForDetail[];
  @Output() openChat: EventEmitter<any> = new EventEmitter<any>();
  constructor() { }

  ngOnInit() {
  }

  onMessageClick(message: MessageForDetail) {
    let chatWithId: number;
    let chatWithName: string;
    switch (this.box) {
      case 'inbox': chatWithId = message.senderId; chatWithName = message.senderName; break;
      case 'outbox': chatWithId = message.recieverId; chatWithName = message.recieverName; break;
      default: chatWithId = message.senderId; chatWithName = message.senderName; break;
    }
    this.openChat.emit({id: chatWithId, name: chatWithName});
  }

}
