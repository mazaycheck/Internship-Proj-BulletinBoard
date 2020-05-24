import { Injectable, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventEmitterService {

constructor() { }
  invokeFirstComponentFunction = new EventEmitter();
  subVar: Subscription;

  onFirstComponentEvent() {
    this.invokeFirstComponentFunction.emit();
  }
}
