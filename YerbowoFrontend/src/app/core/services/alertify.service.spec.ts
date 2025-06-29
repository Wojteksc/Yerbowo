import { TestBed } from '@angular/core/testing';
import { AlertifyService } from './alertify.service';

declare let alertify: any;

describe('AlertifyService', () => {
  let service: AlertifyService;

  const setSpy = jasmine.createSpy('set');
  const confirmCallbackSpy = jasmine.createSpy('confirmCallback');

  const alertifyMock = {
    confirm: jasmine.createSpy('confirm').and.callFake((message: string, callback: (confirmed: boolean) => void) => {
      confirmCallbackSpy.and.callFake(callback);
      return {
        set: setSpy.and.callFake(() => {
          return {
            set: setSpy
          };
        })
      };
    }),
    success: jasmine.createSpy('success'),
    error: jasmine.createSpy('error'),
    warning: jasmine.createSpy('warning'),
    message: jasmine.createSpy('message')
  };

  beforeEach(() => {
    (window as any).alertify = alertifyMock;

    TestBed.configureTestingModule({
      providers: [AlertifyService]
    });

    service = TestBed.inject(AlertifyService);


    setSpy.calls.reset();
    alertifyMock.confirm.calls.reset();
    alertifyMock.success.calls.reset();
    alertifyMock.error.calls.reset();
    alertifyMock.warning.calls.reset();
    alertifyMock.message.calls.reset();
    confirmCallbackSpy.calls.reset();
  });

  it('should call alertify.confirm with correct params and invoke callback on confirm', () => {
    const message = 'Potwierdź akcję';
    const title = 'Uwaga';
    const okCallback = jasmine.createSpy('okCallback');

    service.confirm(message, title, okCallback);

    expect(alertifyMock.confirm).toHaveBeenCalledWith(message, jasmine.any(Function));
    expect(setSpy).toHaveBeenCalledWith({ title });
    expect(setSpy).toHaveBeenCalledWith({ labels: { ok: 'Tak', cancel: 'Nie' } });

    const callback = alertifyMock.confirm.calls.argsFor(0)[1];
    callback(true);
    expect(okCallback).toHaveBeenCalled();

    okCallback.calls.reset();
    callback(false);
    expect(okCallback).not.toHaveBeenCalled();
  });

  it('should call alertify.success with message and default wait time', () => {
    service.success('Sukces!');
    expect(alertifyMock.success).toHaveBeenCalledWith('Sukces!', 5);
  });

  it('should call alertify.success with message and custom wait time', () => {
    service.success('Szybki sukces!', 10);
    expect(alertifyMock.success).toHaveBeenCalledWith('Szybki sukces!', 10);
  });

  it('should call alertify.error with message', () => {
    service.error('Błąd!');
    expect(alertifyMock.error).toHaveBeenCalledWith('Błąd!');
  });

  it('should call alertify.warning with message', () => {
    service.warning('Ostrzeżenie!');
    expect(alertifyMock.warning).toHaveBeenCalledWith('Ostrzeżenie!');
  });

  it('should call alertify.message with message', () => {
    service.message('Informacja!');
    expect(alertifyMock.message).toHaveBeenCalledWith('Informacja!');
  });
});