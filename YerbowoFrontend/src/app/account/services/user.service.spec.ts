import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService } from './user.service';
import { User } from '../../shared/models/user';
import { environment } from 'src/environments/environment';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;

  const baseUrl = environment.apiUrl;
  const userId = 123;

  const mockUser: User = {
    id: userId,
    firstName: 'Anna',
    lastName: 'Kowalska',
    companyName: 'ExampleCorp',
    email: 'anna@example.com',
    confirmEmail: 'anna@example.com',
    newPassword: 'newPass123',
    confirmPassword: 'newPass123',
    currentPassword: 'oldPass123'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService]
    });

    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch user data by ID', () => {
    service.getUser(userId).subscribe(user => {
      expect(user).toEqual(mockUser);
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockUser);
  });

  it('should update user data', () => {
    const updatedUser: User = {
      ...mockUser,
      firstName: 'Joanna',
      lastName: 'Nowak'
    };

    service.updateUser(userId, updatedUser).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedUser);
    req.flush({ success: true });
  });
});
