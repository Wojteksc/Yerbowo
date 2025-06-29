import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { SliderComponent } from './slider.component';
import { By } from '@angular/platform-browser';

describe('SliderComponent', () => {
  let component: SliderComponent;
  let fixture: ComponentFixture<SliderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SliderComponent ],
      imports: [ RouterTestingModule.withRoutes([]) ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SliderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the slider component', () => {
    expect(component).toBeTruthy();
  });

  it('should render slide image with correct src', () => {
    const img = fixture.debugElement.query(By.css('#first-slide-home')).nativeElement as HTMLImageElement;
    expect(img).toBeTruthy();
    expect(img.src).toContain('assets/images/common/slider1.jpg');
  });

  it('should render top image with correct src', () => {
    const topImg = fixture.debugElement.query(By.css('.top-image img')).nativeElement as HTMLImageElement;
    expect(topImg).toBeTruthy();
    expect(topImg.src).toContain('assets/images/common/top-image.jpg');
  });

  it('should render bottom image with correct src', () => {
    const bottomImg = fixture.debugElement.query(By.css('.bottom-image img')).nativeElement as HTMLImageElement;
    expect(bottomImg).toBeTruthy();
    expect(bottomImg.src).toContain('assets/images/common/bottom-image.jpg');
  });
});