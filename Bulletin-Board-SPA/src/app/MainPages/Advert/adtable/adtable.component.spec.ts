/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdtableComponent } from './adtable.component';

describe('AdtableComponent', () => {
  let component: AdtableComponent;
  let fixture: ComponentFixture<AdtableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdtableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdtableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
