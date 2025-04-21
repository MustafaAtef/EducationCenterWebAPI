# Education Center API

## Overview

The Education Center API is a backend service designed to manage various aspects of an education center. It provides endpoints for managing students, teachers, classes, subjects, attendance, grades, expenses, and user authentication. The API is built using .NET 8.0 and follows a modular architecture with services, controllers, and database entities.

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
EducationCenterAPI/
├── Controllers/          # API controllers for handling HTTP requests
├── CustomValidations/    # Custom validation attributes
├── Database/             # Database context and entity models
├── Dtos/                 # Data Transfer Objects
├── Exceptions/           # Custom exception classes and middleware
├── Options/              # Configuration options (e.g., JWT settings)
├── ServiceContracts/     # Interface definitions for services
├── Services/             # Service implementations
├── appsettings.json      # Application configuration
├── Program.cs            # Application entry point
```

## Technologies Used

- **Framework**: .NET 8.0
- **Database**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Dependency Injection**: Built-in .NET DI container

## Setup Instructions

### Prerequisites

- .NET SDK 8.0 or later
- SQL Server (or any compatible database)

### Steps

1. Clone the repository:

   ```bash
   git clone <repository-url>
   cd EducationCenter
   ```

2. Update the database connection string in `appsettings.json`:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "<your-database-connection-string>"
   }
   ```

3. Apply database migrations:

   ```bash
   dotnet ef database update
   ```

4. Run the application:

   ```bash
   dotnet run --project EducationCenterAPI
   ```

5. Access the API at `https://localhost:5001` (or the configured port).

## Key Configuration

### JWT Settings

The JWT settings are configured in `appsettings.json` under the `jwt` section:

```json
"jwt": {
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "SigningKey": "YourSecretKey"
}
```

### Database Configuration

The database connection string is located in `appsettings.json` under the `ConnectionStrings` section.

## API Endpoints

The API provides the following endpoints:

### Authentication

- `POST /auth/login`: Authenticate a user and return a JWT.
- `POST /auth/register`: Register a new user.

### Students

- `GET /students`: Retrieve a list of students.
- `POST /students`: Create a new student.

### Teachers

- `GET /teachers`: Retrieve a list of teachers.
- `POST /teachers`: Create a new teacher.

### Classes

- `GET /classes`: Retrieve a list of classes.
- `POST /classes`: Create a new class.

### Subjects

- `GET /subjects`: Retrieve a list of subjects.
- `POST /subjects`: Create a new subject.

### Attendance

- `GET /attendance`: Retrieve attendance records.
- `POST /attendance`: Record attendance.

### Grades

- `GET /grades`: Retrieve grades.
- `POST /grades`: Record grades.

### Expenses

- `GET /expenses`: Retrieve expenses.
- `POST /expenses`: Record an expense.

## Error Handling

The API uses a global error handling middleware to provide consistent error responses. Custom exceptions like `BadRequestException` and `UniqueException` are used to handle specific error scenarios.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
