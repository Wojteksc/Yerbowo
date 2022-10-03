import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

confirm(message: string, title: string, okCallBack: () => any ) {
  alertify.confirm(message, function(e) {
    if (e) {
      okCallBack();
    } else {}
  }).set({title}).set({labels:{ok:'Tak', cancel: 'Nie'}});
}

success(message: string, wait: number = 5) {
  alertify.success(message, wait);
}

error(message: string) {
  alertify.error(message);
}

warning(message: string) {
  alertify.warning(message);
}

message(message: string) {
  alertify.message(message);
}

}

