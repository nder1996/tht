export class TaskResponse {
    constructor(
      public id?: number,
      public title?: string,
      public description?: string,
      public status?: string,
      public state?: string,
      public createdAt?: Date,
      public updatedAt?: Date,
      public dueDate?: Date
    ) {}
   }