/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdcreateComponent } from './adcreate.component';

describe('AdcreateComponent', () => {
  let component: AdcreateComponent;
  let fixture: ComponentFixture<AdcreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdcreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdcreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
