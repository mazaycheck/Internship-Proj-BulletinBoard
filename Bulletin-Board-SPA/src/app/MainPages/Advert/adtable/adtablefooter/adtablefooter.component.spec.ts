/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdtablefooterComponent } from './adtablefooter.component';

describe('AdtablefooterComponent', () => {
  let component: AdtablefooterComponent;
  let fixture: ComponentFixture<AdtablefooterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdtablefooterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdtablefooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
