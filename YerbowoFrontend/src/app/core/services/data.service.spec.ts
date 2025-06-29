import { TestBed } from '@angular/core/testing';
import { DataService } from './data.service';

describe('DataService', () => {
  let service: DataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with "puste"', (done) => {
    service.cartTotal$.subscribe(value => {
      expect(value).toBe('puste');
      done();
    });
  });

  it('should emit "puste" when total is 0', (done) => {
    service.cartTotal$.subscribe(value => {
      if (value === 'puste') {
        done();
      }
    });

    service.changeTotalCartProducts(0);
  });

  it('should emit number when total is greater than 0', (done) => {
    const testValue = 5;
    service.cartTotal$.subscribe(value => {
      if (value === testValue) {
        done();
      }
    });

    service.changeTotalCartProducts(testValue);
  });
});
