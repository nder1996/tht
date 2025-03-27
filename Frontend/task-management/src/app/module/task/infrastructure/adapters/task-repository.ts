import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/app/environments/environments';
import { ITareaRepositorio } from '../../domain/ITaskRepositorio';
import { TaskRequest } from '../../application/dtos/request/task-request.model';
import { ApiResponse } from '../../application/dtos/response/api-response.model';
import { TaskResponse } from '../../application/dtos/response/task-response';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskRepository implements ITareaRepositorio {

  private apiUrl = `${environment.apiUrl}/Tareas`;

  constructor(private http: HttpClient) { }
  async getAllTask(): Promise<ApiResponse<TaskResponse[]>> {
    return lastValueFrom(this.http.get<ApiResponse<TaskResponse[]>>(`${this.apiUrl}/tasks`));
  }

  async getById(id: number): Promise<ApiResponse<TaskResponse>> {
    return lastValueFrom(this.http.get<ApiResponse<TaskResponse>>(`${this.apiUrl}/tasks/${id}`));
  }

  async inactivateById(id: number): Promise<ApiResponse<string>> {
    return lastValueFrom(this.http.delete<ApiResponse<string>>(`${this.apiUrl}/${id}`));
  }

  async insert(taskRequest: TaskRequest): Promise<ApiResponse<string>> {
    return lastValueFrom(this.http.post<ApiResponse<string>>(this.apiUrl, taskRequest));
  }

  async update(id: number, taskRequest: TaskRequest): Promise<ApiResponse<string>> {
    return lastValueFrom(this.http.put<ApiResponse<string>>(`${this.apiUrl}/${id}`, taskRequest));
  }
  

  
}
