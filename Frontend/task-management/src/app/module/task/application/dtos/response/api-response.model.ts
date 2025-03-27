export class ApiResponse<T> {
    constructor(
      public meta?: ApiResponse.Meta,
      public data?: T,
      public error?: ApiResponse.ErrorDetails
    ) {}
  }
  
  export namespace ApiResponse {
    export class Meta {
      constructor(
        public message?: string,
        public statusCode?: number
      ) {}
    }
    
    export class ErrorDetails {
      constructor(
        public code?: string,
        public description?: string
      ) {}
    }
  }