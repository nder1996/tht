import { TaskRequest } from "../application/dtos/request/task-request.model";
import { ApiResponse } from "../application/dtos/response/api-response.model";
import { TaskResponse } from "../application/dtos/response/task-response";

export interface ITareaRepositorio {
    getAllTask(): Promise<ApiResponse<TaskResponse[]>>;
    getById(id: number): Promise<ApiResponse<TaskResponse>>;
    inactivateById(id: number): Promise<ApiResponse<string>>;
    insert(tareaRequest: TaskRequest): Promise<ApiResponse<string>>;
    update(id:number,tareaRequest: TaskRequest): Promise<ApiResponse<string>>;
  }