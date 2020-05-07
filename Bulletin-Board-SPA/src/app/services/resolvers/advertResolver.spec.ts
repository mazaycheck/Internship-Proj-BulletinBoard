/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AdvertResolver } from './advertResolver';

describe('Service: AdvertResolver', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdvertResolver]
    });
  });

  it('should ...', inject([AdvertResolver], (service: AdvertResolver) => {
    expect(service).toBeTruthy();
  }));
});
