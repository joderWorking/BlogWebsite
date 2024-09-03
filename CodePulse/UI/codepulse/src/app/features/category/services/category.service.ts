import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../models/ApiResponse.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';
import {CookieService} from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient,
              private cookieService:CookieService) { }

  getAllCategories(query?:string, sortBy?:string, sortDirection?:string): Observable<Category[]>{
    let params = new HttpParams();
    if (query) {
      params = params.set('query', query);
    }
    if (sortBy) {
      params = params.set('sortBy', sortBy);
    }
    if(sortDirection){
      params = params.set('sortDirection', sortDirection);
    }
    return this.http.get<Category[]>(`${environment.apiBaseUrl}/api/categories`,
      {
        params: params
      }
    );
  }

  getCategoryById(id:string): Observable<ApiResponse>{
    return this.http.get<ApiResponse>(`${environment.apiBaseUrl}/api/categories/${id}`);
  }
  addCategory(model : AddCategoryRequest) : Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/api/categories?addAuth=true`, model);
  }
  updateCategory(category:UpdateCategoryRequest):Observable<ApiResponse>{
    return this.http.put<ApiResponse>(`${environment.apiBaseUrl}/api/categories?addAuth=true`,category);
  }
  deleteCategory(id:string):Observable<Category>{
    return this.http.delete<Category>(`${environment.apiBaseUrl}/api/categories?addAuth=true/${id}`);
  }
}
