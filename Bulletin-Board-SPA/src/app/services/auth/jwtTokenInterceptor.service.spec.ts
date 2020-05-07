/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { JwtTokenInterceptorService } from './jwtTokenInterceptor.service';

describe('Service: JwtTokenInterceptor', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [JwtTokenInterceptorService]
    });
  });

  it('should ...', inject([JwtTokenInterceptorService], (service: JwtTokenInterceptorService) => {
    expect(service).toBeTruthy();
  }));
});
