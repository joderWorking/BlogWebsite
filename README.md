# CodePulse

## Overview

CodePulse is a comprehensive blogging platform that allows users to create, manage, and publish blog posts. The platform includes features for user authentication, role-based access control, and content management.

## Features

- **User Authentication**: Secure login and registration system.
- **Role-Based Access Control**: Different roles such as Reader and Writer with specific permissions.
- **Blog Management**: Create, update, delete, and view blog posts.
- **Category Management**: Organize blog posts into categories.
- **Image Upload**: Upload and manage images for blog posts.

## Project Structure

### Backend (API)

The backend is built using ASP.NET Core and Entity Framework Core. It provides RESTful APIs for managing users, blog posts, categories, and images.

#### Key Files

- **Controllers**
  - `ImagesController.cs`: Manages image uploads and retrieval.
    ```csharp:API/CodePulse.API/Controllers/ImagesController.cs
    startLine: 1
    endLine: 85
    ```
  - `CategoriesController.cs`: Manages categories for blog posts.
    ```csharp:API/CodePulse.API/Controllers/CategoriesController.cs
    startLine: 1
    endLine: 121
    ```
  - `BlogPostsController.cs`: Manages blog posts.
    ```csharp:API/CodePulse.API/Controllers/BlogPostsController.cs
    startLine: 1
    endLine: 221
    ```

- **Data**
  - `AuthDBContext.cs`: Configures the database context for authentication.
    ```csharp:API/CodePulse.API/Data/EFs/AuthDBContext.cs
    startLine: 1
    endLine: 71
    ```
  - `TokenRepo.cs`: Handles JWT token generation.
    ```csharp:API/CodePulse.API/Data/Repositories/TokenRepo.cs
    startLine: 1
    endLine: 43
    ```

- **Migrations**
  - `Initial Add Migrations Auth.Designer.cs`: Initial database migration for authentication.
    ```csharp:API/CodePulse.API/Migrations/20240831091639_Initial Add Migrations Auth.Designer.cs
    startLine: 1
    endLine: 324
    ```

### Frontend (UI)

The frontend is built using Angular. It provides a user-friendly interface for interacting with the backend APIs.

#### Key Files

- **Guards**
  - `auth.guard.ts`: Protects routes based on user authentication and roles.
    ```typescript:UI/codepulse/src/app/features/Auth/guards/auth.guard.ts
    startLine: 1
    endLine: 39
    ```

- **Components**
  - `navbar.component.html`: Navigation bar with dynamic links based on user roles.
    ```html:UI/codepulse/src/app/core/components/navbar/navbar.component.html
    startLine: 1
    endLine: 44
    ```
  - `blog-detail.component.html`: Displays detailed view of a blog post.
    ```html:UI/codepulse/src/app/features/public/blog-detail/blog-detail.component.html
    startLine: 1
    endLine: 35
    ```
  - `edit-category.component.html`: Form for editing a category.
    ```html:UI/codepulse/src/app/features/category/edit-category/edit-category.component.html
    startLine: 1
    endLine: 43
    ```

- **Server**
  - `server.ts`: Express server configuration for Angular Universal.
    ```typescript:UI/codepulse/server.ts
    startLine: 1
    endLine: 57
    ```

## Getting Started

### Prerequisites

- .NET 6 SDK
- Node.js (v18 or higher)
- Angular CLI

### Setup

1. **Clone the repository**
   ```bash
   git clone [https://github.com/your-repo/codepulse.git](https://github.com/joderWorking/BlogWebsite.git)
   cd codepulse
   ```

2. **Backend Setup**
   - Navigate to the API directory:
     ```bash
     cd API/CodePulse.API
     ```
   - Restore .NET dependencies:
     ```bash
     dotnet restore
     ```
   - Update the database:
     ```bash
     dotnet ef database update
     ```
   - Run the API:
     ```bash
     dotnet run
     ```

3. **Frontend Setup**
   - Navigate to the UI directory:
     ```bash
     cd UI/codepulse
     ```
   - Install Node.js dependencies:
     ```bash
     npm install
     ```
   - Run the Angular application:
     ```bash
     ng serve
     ```
