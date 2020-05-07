/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { BrandCategoryService } from './brandCategory.service';

describe('Service: BrandCategory', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BrandCategoryService]
    });
  });

  it('should ...', inject([BrandCategoryService], (service: BrandCategoryService) => {
    expect(service).toBeTruthy();
  }));
});
