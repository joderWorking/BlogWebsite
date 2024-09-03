import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ImageService } from './image.service';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogImage } from '../../models/blog-image.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-image-selector',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './image-selector.component.html',
  styleUrl: './image-selector.component.css',
})
export class ImageSelectorComponent implements OnInit,OnDestroy {
  private file?: File;
  fileName: string = '';
  title: string = '';
  uploadImageSubscription$?:Subscription;
  images$?:Observable<BlogImage[]>;
  
  @ViewChild('form', {static: false}) imageUpLoadForm?:NgForm;
  constructor(private imageService:ImageService,
              private router: Router
  ){

  }
  ngOnInit(): void {
    this.getImages();
  }
  onFileUploadChange(event: Event): void {
    const element = event.currentTarget as HTMLInputElement;
    this.file = element.files?.[0];
  }
  uploadImage() : void{
    if(this.file && this.fileName !== '' && this.title !== ''){
      this.uploadImageSubscription$ = this.imageService.uploadImage(this.file,this.fileName,this.title)
      .subscribe({
        next: (response) => {
          this.imageUpLoadForm?.resetForm();
          this.getImages();
        }
      });
    }
  }
  selectedImage(image:BlogImage):void{
    this.imageService.selectedImage(image);
    
  }
  ngOnDestroy(): void {
    this.uploadImageSubscription$?.unsubscribe();
  }
  private getImages(){
    this.images$= this.imageService.getAllImage();
  }
}
