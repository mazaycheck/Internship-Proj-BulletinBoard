/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdminMainControlComponent } from './admin-main-control.component';

describe('AdminMainControlComponent', () => {
  let component: AdminMainControlComponent;
  let fixture: ComponentFixture<AdminMainControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminMainControlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminMainControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
