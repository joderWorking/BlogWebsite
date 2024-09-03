import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogPostService } from '../../blog-post/services/blog-post.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../../blog-post/models/blog-post.model';
import { CommonModule } from '@angular/common';
import { MarkdownComponent, MarkdownModule } from 'ngx-markdown';

@Component({
  selector: 'app-blog-detail',
  standalone: true,
  imports: [CommonModule, MarkdownModule, MarkdownComponent],
  templateUrl: './blog-detail.component.html',
  styleUrl: './blog-detail.component.css'
})
export class BlogDetailComponent implements OnInit {
  url:string | null = null;
  blogPost$?:Observable<BlogPost>;
  constructor(private route: ActivatedRoute,
    private blogPostService: BlogPostService
  ){

  }
  ngOnInit(): void {
    this.route.paramMap
    .subscribe({
      next: (params) => {
        this.url = params.get('url')
      }
    });
    //Fetch blog details by URL
    if(this.url){
      this.blogPost$ = this.blogPostService.getBlogPostByUrlHandle(this.url);
    }
  }

}
