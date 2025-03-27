import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TaskComponent } from './module/task/presentation/components/task/task.component';
import { PrincipalComponent } from './module/task/presentation/pages/principal/principal.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TaskModule } from './module/task/task.module';

@NgModule({
  declarations: [
    AppComponent,
    PrincipalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    TaskModule 
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
