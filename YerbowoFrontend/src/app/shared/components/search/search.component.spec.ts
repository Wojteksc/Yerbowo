import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SearchComponent } from './search.component';
import { DataService } from 'src/app/core/services/data.service';
import { BehaviorSubject } from 'rxjs';
import { By } from '@angular/platform-browser';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;
  let dataServiceMock: any;
  let cartTotalSubject: BehaviorSubject<number>;

  beforeEach(async () => {
    cartTotalSubject = new BehaviorSubject<number>(0);

    dataServiceMock = {
      cartTotal$: cartTotalSubject.asObservable()
    };

    await TestBed.configureTestingModule({
      declarations: [ SearchComponent ],
      providers: [
        { provide: DataService, useValue: dataServiceMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should subscribe to cartTotal$ and update totalCartProducts', () => {
    expect(component.totalCartProducts).toBe(0);

    cartTotalSubject.next(5);
    fixture.detectChanges();

    expect(component.totalCartProducts).toBe(5);

    cartTotalSubject.next(10);
    fixture.detectChanges();

    expect(component.totalCartProducts).toBe(10);
  });

  it('should display totalCartProducts in template', () => {
    cartTotalSubject.next(7);
    fixture.detectChanges();

    const cartText = fixture.debugElement.query(By.css('#shopping-cart span')).nativeElement.textContent;
    expect(cartText).toContain('Koszyk');

    const fullText = fixture.debugElement.query(By.css('#shopping-cart')).nativeElement.textContent;
    expect(fullText).toContain('(7)');
  });

  it('should unsubscribe on destroy', () => {
    spyOn(component['subscription']!, 'unsubscribe').and.callThrough();
    component.ngOnDestroy();
    expect(component['subscription']!.unsubscribe).toHaveBeenCalled();
  });
});
