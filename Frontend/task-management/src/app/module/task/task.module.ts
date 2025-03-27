import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MessagesModule } from 'primeng/messages';
import { ConfirmationService, MessageService } from 'primeng/api';
import { TaskRoutingModule } from './task-routing.module';

import { TableModule } from 'primeng/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // Importa estos módulos
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { ToolbarModule } from 'primeng/toolbar';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TaskComponent } from './presentation/components/task/task.component';
import { ConfirmPopupModule } from 'primeng/confirmpopup';


@NgModule({
  declarations: [
    TaskComponent
  ],
  imports: [
    CommonModule,
    TaskRoutingModule,
    MessagesModule,
    TableModule,
    ReactiveFormsModule,
    ButtonModule,
    DialogModule,
    ToastModule,
    TagModule,
    InputTextModule,
    InputTextareaModule,
    DropdownModule,
    ToolbarModule,
    CalendarModule,
    FormsModule,
    ConfirmDialogModule,
    ConfirmPopupModule,
    ToastModule
  ],
  exports: [
    TaskComponent
  ],
  providers: [ConfirmationService, MessageService]
})
export class TaskModule { }
