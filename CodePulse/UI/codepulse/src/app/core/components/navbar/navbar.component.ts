import {Component, OnInit} from '@angular/core';
import { CategoryListComponent } from '../../../features/category/category-list/category-list.component';
import {Router, RouterModule} from '@angular/router';
import {AuthService} from "../../../features/Auth/Services/auth.service";
import {response} from "express";
import {User} from "../../../features/Auth/Models/user.model";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, NgIf],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  user?:User;

  constructor(private authService: AuthService,
              private router: Router) {
  }

  ngOnInit() :void{
    this.authService.user().subscribe({
      next: (response) => {
        this.user = response;
      }
    });
    this.user = this.authService.getUser();
  }

  onLogout() {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }
}
