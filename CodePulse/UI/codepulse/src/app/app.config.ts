import { ApplicationConfig, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import {provideHttpClient, withInterceptors} from '@angular/common/http'
import { MarkdownModule } from 'ngx-markdown';
import {authInterceptor} from "./core/interceptors/auth.interceptor";
export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideClientHydration(),
    provideHttpClient(), importProvidersFrom(MarkdownModule.forRoot()),
    provideHttpClient(withInterceptors([authInterceptor]))
  // <- changed here
  ]
};
