# .NET Project - Forum Project - "BG-Baba"

## Overview

The Forum Project is a robust and feature-rich web application built using ASP.NET Core. It provides a comprehensive platform for users to engage in discussions, share posts, comment on threads, and react to various content. Administrators have access to additional management functionalities, ensuring smooth operation and content moderation.

## Features

- User Registration and Authentication
- Post Creation and Management
- Commenting System
- Reaction System (Likes, Dislikes, etc.)
- Admin Panel for User and Content Management

## Technologies Used

- ASP.NET Core
- Entity Framework Core
- SQL Server
- xUnit and Moq for unit testing
- EF InMemory for integration testing
- AutoMapper for object mapping

## Installation

Follow these steps to set up and run the application:

1. **Clone the repository**
   ```bash
   https://github.com/WebProgramming-MB/ForumProject.git
   cd ForumProject
2. **Configure the database connection**
	Update the appsettings.json file with your SQL Server connection string.
3. **Run the application**
	```
	dotnet run

## Database Diagram

![Alt Text](https://res.cloudinary.com/da0grkml3/image/upload/v1722811990/ayn2msxjlmrypbxiy4m0.png)

## Solution Structure
The project is structured into several key directories and files, each serving a distinct purpose:

1. **Configuration**
Update the appsettings.json file with your SQL Server connection string and other necessary settings. Here is what each section in the appsettings.json file represents:
	```
	{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost; Database=ForumProjectDB;Trusted_Connection=True;MultipleActiveResultSets=True"
    },
    "Jwt": {
        "Key": "your_long_secret_key_here_which_is_32_chars_or_more",
        "Issuer": "issuer",
        "Audience": "audience",
        "ExpireMinutes": 60
    },
    "CloudinarySettings": {
        "CloudName": "your_cloudinary_cloud_name",
        "ApiKey": "your_cloudinary_api_key",
        "ApiSecret": "your_cloudinary_api_secret"
    }
}

2. **Controllers**
Holds the controllers which manage the HTTP requests and responses. For example, AuthController handles authentication-related endpoints.

3. **Data**
Includes the database context and initialization scripts, such as ApplicationDbContext which is used for database operations.

4. **Middlewares**
Contains custom middleware components for handling specific tasks, like ExceptionHandlingMiddleware for global exception handling.

5. **Models**
Defines the data models and query parameters used throughout the application.

6. **Repositories**
Implements the data access layer, encapsulating the logic for accessing data from the database. For instance, PostRepository manages CRUD operations for posts.

7. **Services**
Includes the business logic layer, where services like AuthService and PostService contain the core application logic.

8. **Helpers and Utilities**
Provides various helper classes and utilities used across the application, such as AuthManager for authentication-related tasks.
