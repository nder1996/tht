export class TaskRequest {
    constructor(
        public id?: number,
        public titulo?: string,
        public descripcion?: string,
        public status?: string,
        public state?: string,
        public createdAt?: Date,
        public updatedAt?: Date,
        public due_date?: Date
    ) {}
  }
