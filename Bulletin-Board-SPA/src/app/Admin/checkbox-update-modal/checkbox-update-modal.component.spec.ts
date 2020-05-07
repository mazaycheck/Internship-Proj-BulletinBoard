/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CheckboxUpdateModalComponent } from './checkbox-update-modal.component';

describe('CheckboxUpdateModalComponent', () => {
  let component: CheckboxUpdateModalComponent;
  let fixture: ComponentFixture<CheckboxUpdateModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckboxUpdateModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckboxUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
