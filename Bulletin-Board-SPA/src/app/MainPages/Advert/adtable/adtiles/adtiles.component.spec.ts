/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdtilesComponent } from './adtiles.component';

describe('AdtilesComponent', () => {
  let component: AdtilesComponent;
  let fixture: ComponentFixture<AdtilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdtilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdtilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
