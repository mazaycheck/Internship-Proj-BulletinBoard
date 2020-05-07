/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdtableheaderComponent } from './adtableheader.component';

describe('AdtableheaderComponent', () => {
  let component: AdtableheaderComponent;
  let fixture: ComponentFixture<AdtableheaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdtableheaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdtableheaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
