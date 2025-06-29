import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { NewsletterService } from './newsletter.service';
import { environment } from 'src/environments/environment';

describe('NewsletterService', () => {
  let service: NewsletterService;
  let httpMock: HttpTestingController;
  const baseUrl = `${environment.apiUrl}newsletter/`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [NewsletterService],
    });
    service = TestBed.inject(NewsletterService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('#invite', () => {
    it('should POST email and return string response', () => {
      const email = 'test@example.com';
      const mockResponse = 'Invite sent';

      service.invite(email).subscribe((res) => {
        expect(res).toBe(mockResponse);
      });

      const req = httpMock.expectOne(baseUrl + 'invite');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toBe(email);
      req.flush(mockResponse);
    });
  });

  describe('#subscribe', () => {
    it('should POST payload and return response', () => {
      const payload = { email: 'test@example.com' };
      const mockResponse = { success: true };

      service.subscribe(payload).subscribe((res) => {
        expect(res).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(baseUrl + 'subscribe');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(payload);
      req.flush(mockResponse);
    });
  });

  describe('#unsubscribe', () => {
    it('should POST payload and return response', () => {
      const payload = { email: 'test@example.com' };
      const mockResponse = { success: true };

      service.unsubscribe(payload).subscribe((res) => {
        expect(res).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(baseUrl + 'unsubscribe');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(payload);
      req.flush(mockResponse);
    });
  });
});
