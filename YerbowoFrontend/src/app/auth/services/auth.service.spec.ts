import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { environment } from 'src/environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  const baseUrl = `${environment.apiUrl}auth/`;

  const fakeJwt = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.' +
                  'eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.' +
                  'SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('login', () => {
    it('should store token and photoUrl on successful login', () => {
      const mockResponse = {
        token: { token: fakeJwt },
        photoUrl: 'http://photo.url'
      };

      spyOn<any>(service, 'handleAuthResponse').and.callThrough();

      service.login({ email: 'test@test.com', password: '123456' }).subscribe(() => {
        expect(localStorage.getItem('token')).toBe(mockResponse.token.token);
        expect(localStorage.getItem('photoUrl')).toBe(mockResponse.photoUrl);
        expect(service['decodedToken']).toBeTruthy();
        expect(service['photoUrl']).toBe(mockResponse.photoUrl);
      });

      const req = httpMock.expectOne(baseUrl + 'login');
      expect(req.request.method).toBe('POST');
      req.flush(mockResponse);
    });
  });

  describe('loginWithSocial', () => {
    it('should call correct endpoint and store token/photo', () => {
      const response = { token: { token: fakeJwt }, photoUrl: 'photo-url' };

      service.loginWithSocial({ provider: 'google' }).subscribe(() => {
        expect(localStorage.getItem('token')).toBe(fakeJwt);
        expect(localStorage.getItem('photoUrl')).toBe('photo-url');
      });

      const req = httpMock.expectOne(baseUrl + 'socialLogin');
      expect(req.request.method).toBe('POST');
      req.flush(response);
    });
  });

  describe('signOut', () => {
    it('should clear localStorage and service fields', () => {
      localStorage.setItem('token', 'token');
      localStorage.setItem('photoUrl', 'photoUrl');
      service['decodedToken'] = { name: 'test' };
      service['photoUrl'] = 'photoUrl';

      service.signOut();

      expect(localStorage.getItem('token')).toBeNull();
      expect(localStorage.getItem('photoUrl')).toBeNull();
      expect(service['decodedToken']).toBeNull();
      expect(service['photoUrl']).toBeNull();
    });
  });

  describe('isLoggedIn', () => {
    it('should return false if no token', () => {
      expect(service.isLoggedIn()).toBeFalse();
    });

    it('should return false if token is expired', () => {
      spyOn<any>(service['jwtHelper'], 'isTokenExpired').and.returnValue(true);
      localStorage.setItem('token', 'expired-token');
      expect(service.isLoggedIn()).toBeFalse();
    });

    it('should return true if token is valid', () => {
      spyOn<any>(service['jwtHelper'], 'isTokenExpired').and.returnValue(false);
      localStorage.setItem('token', 'valid-token');
      expect(service.isLoggedIn()).toBeTrue();
    });
  });

  describe('register', () => {
    it('should post user to register endpoint', () => {
      const user = { email: 'test@test.com', password: '123' };

      service.register(user as any).subscribe(res => {
        expect(res).toEqual({ message: 'ok' });
      });

      const req = httpMock.expectOne(baseUrl + 'register');
      expect(req.request.method).toBe('POST');
      req.flush({ message: 'ok' });
    });
  });

  describe('confirmEmail', () => {
    it('should post email data to confirmEmail endpoint', () => {
      const data = { email: 'a@b.com', token: 'abc' };

      service.confirmEmail(data).subscribe(res => {
        expect(res).toEqual({ confirmed: true });
      });

      const req = httpMock.expectOne(baseUrl + 'confirmEmail');
      expect(req.request.method).toBe('POST');
      req.flush({ confirmed: true });
    });
  });

  describe('restoreSession', () => {
    it('should restore session from localStorage', () => {
      localStorage.setItem('token', fakeJwt);
      localStorage.setItem('photoUrl', 'http://image.com');

      spyOn<any>(service['jwtHelper'], 'decodeToken').and.returnValue({ name: 'User' });

      service.restoreSession();

      expect(service.decodedToken).toEqual({ name: 'User' });
      expect(service.photoUrl).toEqual('http://image.com');
    });
  });
});