import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {AddCategoryRequest} from '../models/add-category-request.model';
import { CategoryListComponent } from '../category-list/category-list.component';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.css'
})
export class AddCategoryComponent implements OnDestroy{
  model: AddCategoryRequest;
  private addCategorySubcription?: Subscription;
  constructor(private catergoryService: CategoryService, 
    private router: Router)
  {
    this.model = {
      Name: '',
      UrlHandle: ''
    };
  }

  onFormSubmit(){ 
    this.addCategorySubcription = this.catergoryService.addCategory(this.model)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/categories');
          }
        });
  }
  ngOnDestroy(): void {
    this.addCategorySubcription?.unsubscribe();
  }
}
