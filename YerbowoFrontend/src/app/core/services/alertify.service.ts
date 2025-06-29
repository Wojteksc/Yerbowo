import { Injectable } from '@angular/core';

declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  confirm(message: string, title: string, okCallBack: () => void): void {
    alertify
      .confirm(message, (confirmed: boolean) => {
        if (confirmed) {
          okCallBack();
        }
      })
      .set({ title })
      .set({ labels: { ok: 'Tak', cancel: 'Nie' } });
  }

  success(message: string, wait: number = 5): void {
    alertify.success(message, wait);
  }

  error(message: string): void {
    alertify.error(message);
  }

  warning(message: string): void {
    alertify.warning(message);
  }

  message(message: string): void {
    alertify.message(message);
  }
}