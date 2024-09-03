import { Category } from "./category.model";

export interface ApiResponse {
    data: Category;
    message: string;
    statusCode: number;
  }