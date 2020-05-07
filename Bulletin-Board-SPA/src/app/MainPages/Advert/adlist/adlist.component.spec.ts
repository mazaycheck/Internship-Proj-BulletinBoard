/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdlistComponent } from './adlist.component';

describe('AdlistComponent', () => {
  let component: AdlistComponent;
  let fixture: ComponentFixture<AdlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
