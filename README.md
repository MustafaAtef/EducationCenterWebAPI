# Education Center API

## Overview

The Education Center API is a backend service designed to manage various aspects of an education center. It provides endpoints for managing students, teachers, classes, subjects, attendance, grades, expenses, and user authentication. Built following Clean Architecture principles, it ensures scalability, maintainability, and testability.

## Features

- **Authentication and Authorization**: JWT-based authentication with role-based access control.
- **Student Management**: Manage students data and fees.
- **Teacher Management**: Manage teachers data, taught subject and salaries.
- **Class Management**: Handle class schedules.
- **Subject Management**: Manage subjects data.
- **Attendance Tracking**: Record and retrieve attendance data.
- **Expense Tracking**: Track expenses and categorize them.
- **Global Error Handling**: Middleware for consistent error responses.

## Project Structure

The project is organized as follows:

```
EducationCenter/
├── Src/
│   ├── EducationCenter.Application/
│   │   ├── CustomValidations/    # Custom validation attributes
│   │   ├── Dtos/                 # Data Transfer Objects
│   │   ├── ServiceContracts/     # Interface definitions for services
│   │   ├── Services/             # Service implementations
│   ├── EducationCenter.Core/
│   │   ├── Entities/             # Core entity models
│   │   ├── Enumerations/         # Enumerations used in the application
│   │   ├── Exceptions/           # Custom exception classes
│   │   ├── RepositoryContracts/  # Repository interface definitions
│   ├── EducationCenter.Infrastructure/
│   │   ├── Auth/                 # Authentication-related logic
│   │   ├── Database/             # Database context and migrations
│   │   ├── Repositories/         # Repository implementations
│   ├── EducationCenter.WebApi/
│   │   ├── Controllers/          # API controllers for handling HTTP requests
│   │   ├── Middlewares/          # Middleware for request handling
│   │   ├── appsettings.json      # Application configuration
│   │   ├── Program.cs            # Application entry point
├── EducationCenter.sln           # Solution file
```

## Technologies Used

- **Framework**: .NET 8.0
- **Database ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **API Documentation**: Swagger/OpenAPI
- **Dependency Injection**: Built-in .NET DI container
