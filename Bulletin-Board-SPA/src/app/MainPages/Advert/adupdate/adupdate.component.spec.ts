/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdupdateComponent } from './adupdate.component';

describe('AdupdateComponent', () => {
  let component: AdupdateComponent;
  let fixture: ComponentFixture<AdupdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdupdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdupdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
