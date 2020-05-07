/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { GenericEntityComponent } from './GenericEntity.component';

describe('GenericEntityComponent', () => {
  let component: GenericEntityComponent;
  let fixture: ComponentFixture<GenericEntityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenericEntityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenericEntityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
