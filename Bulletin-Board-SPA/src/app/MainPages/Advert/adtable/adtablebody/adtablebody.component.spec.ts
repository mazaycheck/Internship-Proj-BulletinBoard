/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdtablebodyComponent } from './adtablebody.component';

describe('AdtablebodyComponent', () => {
  let component: AdtablebodyComponent;
  let fixture: ComponentFixture<AdtablebodyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdtablebodyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdtablebodyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
