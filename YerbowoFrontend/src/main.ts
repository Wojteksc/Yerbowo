import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

import { create } from 'rxjs-spy';

if (environment.production) {
  enableProdMode();
}

//rxjs-spy
const spy = create();
(window as any).spy = spy;
spy.log();

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.log(err));

