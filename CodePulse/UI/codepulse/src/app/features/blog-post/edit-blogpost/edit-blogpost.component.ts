import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MarkdownComponent, MarkdownModule } from 'ngx-markdown';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';
import { response } from 'express';
import { ImageSelectorComponent } from "../../../shared/components/image-selector/image-selector.component";
import { ImageService } from '../../../shared/components/image-selector/image.service';
@Component({
  selector: 'app-edit-blogpost',
  standalone: true,
  imports: [CommonModule, FormsModule, MarkdownModule, MarkdownComponent, ImageSelectorComponent],
  templateUrl: './edit-blogpost.component.html',
  styleUrl: './edit-blogpost.component.css'
})
export class EditBlogpostComponent implements OnInit, OnDestroy{
  id: string | null = null;
  model?: BlogPost;
  categories$?:Observable<Category[]>
  selectedCategories?: string[]
  isImageSelectorVisible: boolean = false;
  routeSubcription?: Subscription;
  updateBlogPostSubcription?: Subscription;
  getBlogPostSubcription?: Subscription;
  deleteBlogPostSubcription?: Subscription;
  imageSelectSubscription?:Subscription;
  
  constructor(private route: ActivatedRoute,
              private blogPostService: BlogPostService,
              private categoryService: CategoryService,
              private router: Router,
              private imageService: ImageService
  ){

  }
  ngOnInit(): void {
    this.routeSubcription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');
        
        // Get blogpost from the API
        if(this.id){
          this.categories$ = this.categoryService.getAllCategories();
          this.getBlogPostSubcription = this.blogPostService.getBlogPostById(this.id).subscribe({
            next: (response) => {
              console.log(response);
              this.model = response;
              this.selectedCategories = response.categories.map(x => x.id);
            }
          });
        }
        this.imageSelectSubscription = this.imageService.onSelectImage()
        .subscribe({
          next: (response) => {
            if(this.model){
              this.model.featuredImageUrl = response.url;
            }
            this.isImageSelectorVisible = false;
          }
            
        })
      }
    })
  }
  onFormSubmit():void{
    //Convert model to request obj
    if(this.model && this.id){
      var updateBlogPost : UpdateBlogPost = {
        author : this.model.author, 
        content : this.model.content,
        shortDescription: this.model.shortDescription,
        featuredImageUrl : this.model.featuredImageUrl,
        isVisible : this.model.isVisible,
        publishedDate : this.model.publishedDate,
        title: this.model.title,
        urlHandle: this.model.urlHandle,
        categories: this.selectedCategories ??[]
      };
      this.updateBlogPostSubcription = this.blogPostService.updateBlogPost(this.id, updateBlogPost)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/blogposts');
        }
      })
    }
  }
  onDelete():void{
    if(this.id){
      this.deleteBlogPostSubcription = this.blogPostService.deleteBlogPost(this.id)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/blogposts');
        }
      })
    }
  } 
  openImageSelector():void{
    this.isImageSelectorVisible = true;
  }
  closeImageSelector():void{
    this.isImageSelectorVisible = false;
  }
  ngOnDestroy(): void {
    this.routeSubcription?.unsubscribe();
    this.updateBlogPostSubcription?.unsubscribe();
    this.getBlogPostSubcription?.unsubscribe(); 
    this.deleteBlogPostSubcription?.unsubscribe();
    this.imageSelectSubscription?.unsubscribe();
  }

}
