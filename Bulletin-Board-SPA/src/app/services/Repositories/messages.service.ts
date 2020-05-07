import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { MessageForCreate } from 'src/app/Models/MessageForCreate';
import { MessageForDetail } from 'src/app/Models/MessageForDetail';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  baseUrl: string;
  controllerPath = 'api/messages/';
  inbox = 'box/inbox';
  unread = 'box/unread';
  outbox = 'box/outbox';
  thread = 'thread/';
  constructor(private http: HttpClient) {
    this.baseUrl = environment.baseUrl + this.controllerPath;
   }

  getInboxMessages(): Observable<any> {
     return this.http.get(this.baseUrl + this.inbox);
  }

  getUnreadMessages(): Observable<any> {
     return this.http.get(this.baseUrl + this.unread);
  }

  getOutboxMessages(): Observable<any> {
    return this.http.get(this.baseUrl + this.outbox);
  }

  getMessageById(id: number): Observable<MessageForDetail> {
    return this.http.get<MessageForDetail>(this.baseUrl + id);
  }

  createMessage(message: MessageForCreate): Observable<any> {
    return this.http.post(this.baseUrl, message);
  }

  markAsRead(id: number): Observable<any> {
    return this.http.put(this.baseUrl + id, null);
  }

  getMessageThread(withUserTwoId: number): Observable<any> {
    return this.http.get(this.baseUrl + this.thread + withUserTwoId);
  }

}
