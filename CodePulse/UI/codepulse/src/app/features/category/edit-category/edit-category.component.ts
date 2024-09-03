import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { response } from 'express';
import { Category } from '../models/category.model';
import { CommonModule } from '@angular/common';
import { ApiResponse } from '../models/ApiResponse.model';
import { FormsModule }   from '@angular/forms';
import { UpdateCategoryRequest } from '../models/update-category-request.model';

@Component({
  selector: 'app-edit-category',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-category.component.html',
  styleUrl: './edit-category.component.css'
})
export class EditCategoryComponent implements OnInit, OnDestroy{
  
  id: string | null = null;
  paramsSubscription?: Subscription;
  editCategorySubcription?: Subscription;
  category?: Category;
  
  constructor(private route: ActivatedRoute,
    private categoryService: CategoryService,
    private router: Router
  ) {

  }

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id){
          //Get the data from the API for the category Id
          this.categoryService.getCategoryById(this.id)
          .subscribe({
            next: (response:ApiResponse) => {
              this.category = response.data;
            }
          });
        }
      }
    });
  }
  onFormSubmit():void{
    const UpdateCategoryRequest: UpdateCategoryRequest = {
      id: this.category?.id ?? '',
      name: this.category?.name??'',
      urlHandle: this.category?.urlHandle??''
    }
    if(this.id){
      this.editCategorySubcription = this.categoryService.updateCategory(UpdateCategoryRequest)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/categories');
        }
      });
    }
    
  }
  onDelete():void{
    if(this.id){
      this.categoryService.deleteCategory(this.id)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/categories');
        }
      })
    }

  }
  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.editCategorySubcription?.unsubscribe();
  }

}
