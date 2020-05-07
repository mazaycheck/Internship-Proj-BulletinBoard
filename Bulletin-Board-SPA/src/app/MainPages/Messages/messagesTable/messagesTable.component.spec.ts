/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MessagesTableComponent } from './messagesTable.component';

describe('MessagesTableComponent', () => {
  let component: MessagesTableComponent;
  let fixture: ComponentFixture<MessagesTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessagesTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessagesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
