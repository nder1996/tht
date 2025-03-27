import { Component } from '@angular/core';
import { TaskResponse } from '../../../application/dtos/response/task-response';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent {

  listTask: TaskResponse[] = [];
  taskForm!: FormGroup;
  actionEditarCrud: boolean = false;
  minDate: Date = new Date();
  showTask: boolean = false;


  constructor(
    private fb: FormBuilder,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    // ... Otros servicios
  ) { 
    this.createForm();
  }

  createForm(): void {
    this.taskForm = this.fb.group({
      id: [null],
      title: ['', Validators.required],
      description: ['', Validators.required],
      dueDate: [null, Validators.required],
      status: ['', Validators.required],
      state: ['', Validators.required],
      createdAt: [null],
      updatedAt: [null]
    });
  }


  cargarModalEditar(task: TaskResponse): void {
    this.actionEditarCrud = true;
    //this.resetForm();
    
    this.taskForm.patchValue({
      id: task.id,
      title: task.title,
      description: task.description,
      dueDate: task.dueDate ? new Date(task.dueDate) : null,
      status: task.status,
      state: task.state,
      createdAt: task.createdAt,
      updatedAt: task.updatedAt
    });
    
    this.showTask = true;
  }

  getStatusSeverity(state: string): string {
    switch (state) {
      case 'PENDIENTE': return 'danger';
      case 'EN_PROGRESO': return 'warning';
      case 'COMPLETADA': return 'success';
      default: return 'info';
    }
  }

  loadTasks(): void {
    this.listTask = [
      new TaskResponse(1, 'Tarea 1', 'Descripción 1', 'PENDIENTE', 'PENDIENTE', new Date(), new Date(), new Date(2023, 12, 31)),
      new TaskResponse(2, 'Tarea 2', 'Descripción 2', 'EN_PROGRESO', 'EN_PROGRESO', new Date(), new Date(), new Date(2023, 11, 15)),
      new TaskResponse(2, 'Tarea 2', 'Descripción 2', 'COMPLETADA', 'EN_PROGRESO', new Date(), new Date(), new Date(2023, 11, 15))
    ];
  }


  ngOnInit(): void {
    this.loadTasks();
  }

  onSubmit(): void {
    if (this.taskForm.invalid) return;
    
    const taskData = new TaskResponse(
      this.taskForm.get('id')?.value,
      this.taskForm.get('title')?.value,
      this.taskForm.get('description')?.value,
      this.taskForm.get('status')?.value,
      this.taskForm.get('state')?.value,
      this.taskForm.get('createdAt')?.value || new Date(),
      new Date(), // updatedAt siempre es la fecha actual
      this.taskForm.get('dueDate')?.value
    );
    
    if (this.actionEditarCrud) {
      // Implementa llamada al servicio para actualizar
      // this.taskService.updateTask(taskData).subscribe(result => {
      //   this.loadTasks();
      //   this.showTask = false;
      // });
      console.log('Actualizar tarea:', taskData);
      this.showTask = false;
    } else {
      // Implementa llamada al servicio para crear
      // this.taskService.createTask(taskData).subscribe(result => {
      //   this.loadTasks();
      //   this.showTask = false;
      // });
      console.log('Crear tarea:', taskData);
      this.showTask = false;
    }
  }


  eliminarTarea(event: Event, tarea: TaskResponse) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: '¿Estás seguro de eliminar esta tarea?',
      header: 'Confirmar eliminación',
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass: "p-button-danger p-button-text",
      rejectButtonStyleClass: "p-button-text p-button-text",
      acceptIcon: "none",
      rejectIcon: "none",
      accept: () => {
        /*this.tareasService.eliminarTarea(tarea.id).subscribe(() => {
          this.messageService.add({ 
            severity: 'success', 
            summary: 'Confirmado', 
            detail: 'Tarea eliminada correctamente' 
          });
          // Actualizar la lista de tareas
          //this.cargarTareas();
        });*/
      },
      reject: () => {
        this.messageService.add({ 
          severity: 'info', 
          summary: 'Rechazado', 
          detail: 'Operación cancelada' 
        });
      }
    });
  }
}
