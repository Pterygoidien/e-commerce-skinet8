import { NgModule } from '@angular/core';
import { AppLayoutModule } from './layout/app-layout.module';

@NgModule({
  imports: [AppLayoutModule],
  exports: [AppLayoutModule],
})
export class AppCoreModule {}
