import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { AuthInterceptor } from './interceptors/auth.interceptor';

// Import Clarity Core Icons
import '@cds/core/icon/register.js';
import { ClarityIcons, userIcon, homeIcon, searchIcon, cogIcon, 
         clockIcon, eyeIcon, listIcon, bellIcon, refreshIcon, 
         checkIcon, timesIcon, plusIcon, pencilIcon, trashIcon, 
         infoCircleIcon, arrowIcon } from '@cds/core/icon';

// Register Clarity Icons
ClarityIcons.addIcons(
  userIcon, homeIcon, searchIcon, cogIcon, clockIcon, eyeIcon, 
  listIcon, bellIcon, refreshIcon, checkIcon, timesIcon, 
  plusIcon, pencilIcon, trashIcon, infoCircleIcon, arrowIcon
);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideAnimations(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    provideClientHydration(withEventReplay())
  ]
};
