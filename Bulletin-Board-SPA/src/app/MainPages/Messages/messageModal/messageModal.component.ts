import { Component, OnInit, ViewChildren, ViewChild, AfterViewInit, OnChanges, SimpleChange, SimpleChanges, AfterViewChecked, OnDestroy } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { MessagesService } from 'src/app/services/Repositories/messages.service';
import { MessageForDetail } from 'src/app/Models/MessageForDetail';
import { MessageForCreate } from 'src/app/Models/MessageForCreate';
import { ChangeDetectionStrategy } from '@angular/core';
import { CdkScrollable } from '@angular/cdk/overlay';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { interval, Subscription } from 'rxjs';
import * as signalR from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-messageModal',
  templateUrl: './messageModal.component.html',
  styleUrls: ['./messageModal.component.css'],
})
export class MessageModalComponent implements OnInit, OnChanges, AfterViewInit, AfterViewChecked, OnDestroy {
  messages: MessageForDetail[] = [];
  newMessage: MessageForCreate;
  refreshSubscription: Subscription = new Subscription();
  hubconnection: signalR.HubConnection;
  baseUrl: string;
  hubData: any;
  userToken: string;
  otherUserIsOnline = false;
  otherUserIsTyping = false;
  timeOutForTyping: any;
  @ViewChild(CdkVirtualScrollViewport) viewPort: CdkVirtualScrollViewport;

  constructor(private messageService: MessagesService, @Inject(MAT_DIALOG_DATA) public data, private authService: AuthService) {
    this.baseUrl = environment.baseUrl;
  }
  startConnection() {
    this.hubconnection = new signalR.HubConnectionBuilder().withUrl(this.baseUrl + 'chatHub',
      { accessTokenFactory: () => this.userToken })
      .build();
    this.hubconnection.start().then(() => console.log('signalR conenction started!')).catch((err) => console.log(err));
  }

  addDataListening() {
    this.hubconnection.on('ReceiveMessage', (signalrdata) => {
      // this.hubData = signalrdata;
      console.log(`SIGNAL : ${signalrdata}`);
      // this.userNotTyping();
      this.updateMessages();

    });
    this.hubconnection.on('OnUserTypesMessage', (signaldata) => {
      this.userTyping();
    });
  }

  userTyping() {
    this.otherUserIsTyping = true;
    if (this.timeOutForTyping) {
      clearTimeout(this.timeOutForTyping);
      this.timeOutForTyping = null;
    }
    this.timeOutForTyping = setTimeout(() => this.otherUserIsTyping = false, 7000);
  }

  userNotTyping() {
    this.otherUserIsTyping = false;
    clearTimeout(this.timeOutForTyping);
    this.timeOutForTyping = null;
  }

  ngOnDestroy(): void {
    this.refreshSubscription.unsubscribe();
  }

  ngAfterViewChecked(): void {
     this.scrollDown();
  }


  ngOnInit() {
    this.userToken = this.authService.getToken();
    this.startConnection();
    this.addDataListening();

    console.log(this.data);
    this.newMessage = { recieverId: this.data.id, text: '' };
    this.updateMessages();

  }

  updateMessages() {
    this.messageService.getMessageThread(this.data.id).subscribe(response => {
      this.userNotTyping();
      this.messages = response;
    });
  }


  ngOnChanges(changes: SimpleChanges) {

  }

  ngAfterViewInit(): void {


  }

  sendMessage() {
    this.messageService.createMessage(this.newMessage).subscribe(response => {
      this.messages.push(response);
      console.log(response);
      this.hubconnection.invoke('SendPrivateMessage', `${this.data.id}`, this.newMessage.text);
      this.updateMessages();
      this.resetMessage();
    });

  }

  resetMessage() {
    this.newMessage.text = '';
  }

  scrollDown() {
    if (this.viewPort) {
      this.viewPort.scrollToIndex(9999999999, 'auto');
    }
  }

  keyPress($event: KeyboardEvent) {
    this.hubconnection.invoke('UserTypesMessage', `${this.data.id}`);
    if ($event.key === 'Enter' && this.newMessage) {
      this.sendMessage();
    }
  }

}
