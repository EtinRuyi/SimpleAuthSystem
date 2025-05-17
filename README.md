# SimpleAuthSystem API

## Simple Authentication API Documentation

### Project Overview
The Simple Authentication API is a secure, microservice-based authentication solution built with ASP.NET Core Web API (8.0), implementing the Onion Architecture and Repository pattern for Database access, Password security with BCrypt, Serilog for Error-Logging and Fluent Validations for validating User Inputs. The system provides essential authentication functionalities including user registration, login with JWT token generation, and endpoint protection via token validation.

### Project Architecture
This project follows the Onion Architecture with clear separation of concerns across multiple layers.

#### Domain Layer (Core:\\ SimpleAuthSystem.Domain)
- Contains entities, interfaces, and domain logic
- Defines contracts for repositories
- No dependencies on other project layers

#### Application Layer (Core:\\ SimpleAuthSystem.Application)
- Contains DTOs (Data Transfer Objects)
- API Response, Mappers, and Validators
- Service interfaces
- Depends only on the Domain layer

#### Infrastructure Layer (Infrastructure:\\ SimpleAuthSystem.Infrastructure)
- Contains implementations of repositories and services
- Database context
- External service integrations (Password hashing, token services)
- Depends on Application and Domain layers

#### Presentation Layer (Presentation:\\ SimpleAuthSystem.API)
- Contains controllers, middleware, and configuration
- Depends on Application and Infrastructure layers for business operations

### Core Features

#### Registration
- Server validates input using Fluent Validation 
- Password is hashed using BCrypt 
- User is saved to the database 

#### Login
- Server verifies credentials with email and password against stored data
- If valid, a JWT token is generated and returned with user information
- The user's Last-Login timestamp is updated

#### Token Validation
- Token validation for protected endpoints
- Current User Details
- Protected endpoint to retrieve current user information

### API Endpoints

#### User Registration
**POST** `/SimpleAuthSystem/api/v1/register`

Registers a new user with a unique username and email.

Postman Request: Auth > Register User

RequestBody:
```json
{
  "username": "john_doe",
  "email": "john.doe@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!"
}
```

#### User Login
**POST** `/SimpleAuthSystem/api/v1/login`

Authenticates a user and returns a JWT token.

Postman Request: Auth > Login User

RequestBody:
```json
{
  "email": "john.doe@example.com",
  "password": "Password123!"
}
```

#### Token Validation
**GET** `/SimpleAuthSystem/api/v1/validatetoken`

Validates a JWT token. Requires Bearer token in the Authorization header.

Postman Request: Auth > Token Validation

Headers: `Authorization: Bearer <token>`

#### Get Current User
**GET** `/SimpleAuthSystem/api/v1/currentuser`

Retrieves the authenticated user's details. Requires Bearer token.

Postman Request: Auth > Current Logged In User

Headers: `Authorization: Bearer <token>`

#### Get User by ID
**GET** `/SimpleAuthSystem/api/v1/users/{id}`

Retrieves a user by their ID. Requires Bearer token.

Postman Request: User > Users{Id}

Headers: `Authorization: Bearer <token>`

#### Get User by Email
**GET** `/SimpleAuthSystem/api/v1/users/email/{email}`

Retrieves a user by their email. Requires Bearer token.

Postman Request: User > Users{Email}

Headers: `Authorization: Bearer <token>`

#### Get All Users (Paginated)
**GET** `/SimpleAuthSystem/api/v1/allusers?page={page}&pageSize={pageSize}`

Retrieves a paginated list of users. Requires Bearer token.

Postman Request: User > Users

Headers: `Authorization: Bearer <token>`

Query Parameters: 
- `page`: Page number (default: 1)
- `pageSize`: Users per page (default: 10, max: 50)

### Setup and Configuration (Getting Started)

#### Prerequisites
- .NET 8.0 SDK
- SQL Server (MSSQL)
- Visual Studio 2022 or VS Code
- Postman (for testing with the provided collection)
- Git (for repository management)

#### Installation Steps
- Clone the repository: `git clone https://github.com/EtinRuyi/SimpleAuthSystem.git`

#### Configuration
- Update database connection. Modify the connection string in appsettings.json:
```json
"ConnectionStrings": {
  "DefaultConnection": "server=.;Database=SimpleAuthSystemDB;Trusted_Connection=True;TrustServerCertificate=True"
}
```
- Update Serilog:WriteTo:Args:path if the log file path (C:/temp/AuthSystLogs.txt) is not suitable.
- Update the JWT configuration:
```json
"JwtConfig": {
  "Secret": "your-secret-key",
  "Issuer": "AuthenticationSystem",
  "Audience": "AuthenticationSystemClient",
  "ExpirationInMinutes": 60
}
```

#### Run database migrations
- Bash: `dotnet ef database update`
 
#### Run the application
- Bash: `dotnet run`
- Or use Visual Studio

#### API Testing/Usage
- Swagger UI is available (in development mode) at: http://localhost:5224/swagger/index.html
- Postman Collection:
  - Postman collection ("Simple-Auth-API.postman_collection.json") provides pre-configured requests for testing all API endpoints.
  - The Postman collection organizes requests into two folders: Auth and User.
  - It uses a variable AuthBaseUrl (optionally set AuthBaseUrl to your API base URL (e.g., http://localhost:5224/SimpleAuthSystem/api/v1/)).
  - Import the file into Postman to get started.
  - The Postman route is: `":\\SimpleAuthSystem\Simple-Auth-API.postman_collection.json"`
  - Additionally, a Postman Collection is added as attachment to the replied mail.

#### Usage Tips
- Register a user first and obtain token
- Use the login endpoint to get a JWT token
- Copy the token from the login response and update the Bearer token in protected requests
- Set the token in the Authorization header for protected endpoints
- Test token validation 
- Run requests and verify responses