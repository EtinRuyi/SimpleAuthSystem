# SimpleAuthSystem API
A simple robust and secure authentication system built with .NET Core, adhering to the Onion Architecture and Repository Pattern. 
This microservice-based API provides user registration, login, and JWT-based token validation, with additional features like custom middleware, logging, and dependency injection for maintainability and scalability.

Features
User Registration: Securely store user credentials with BCrypt password hashing.
User Login: Validate credentials and issue JWT tokens for authentication.
Token Validation: Protect endpoints with JWT middleware.
Onion Architecture: Ensures separation of concerns across domain, application, infrastructure, and presentation layers.
Repository Pattern: Abstracts data access for clean and testable code.
Custom Middleware: Handles exceptions and standardizes API responses.
Serilog Logging: Provides detailed request and error logging.
AutoMapper: Simplifies object mapping between DTOs and entities.
Unit of Work: Manages database transactions efficiently.
Asynchronous Operations: All methods are async for better performance.

Project Structure
The project follows the Onion Architecture with the following layers:

Domain: Core entities (User) and interfaces (IUserRepository, IUnitOfWork).
Application: Business logic, DTOs (RegisterDto, LoginDto, AuthResponseDto), services (AuthService), and AutoMapper profiles.
Infrastructure: Data access layer with Entity Framework Core, generic repositories, and unit of work implementation.
API: Presentation layer with controllers (AuthController), custom middleware (ExceptionMiddleware), and JWT configuration.

Technologies
.NET 8: Core framework for building the API.
Entity Framework Core: ORM for database operations.
SQL Server: Database for storing user data (configurable for other databases).
BCrypt: Secure password hashing.
JWT: Token-based authentication.
AutoMapper: Object-to-object mapping.
Serilog: Structured logging.
Swashbuckle: Swagger UI for API documentation.
Postman: API testing.

Setup Instructions
Follow these steps to set up and run the project locally:

Clone the Repository:
git clone https://github.com/your-username/SimpleAuthSystem.git
cd SimpleAuthSystem


Install Dependencies:Ensure you have the .NET 8 SDK installed. Restore NuGet packages:
dotnet restore


Configure the Database:
Update the connection string in AuthenticationSystem.API/appsettings.json to point to your SQL Server instance.
Example:"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=AuthSystemDb;Trusted_Connection=True;"
}

Apply Migrations:Create and apply database migrations:
cd AuthenticationSystem.API
dotnet ef migrations add InitialCreate -c AppDbContext -o Data/Migrations
dotnet ef database update

Configure JWT Settings:Update the JWT configuration in appsettings.json with a secure key:
"Jwt": {
  "Key": "YourSuperSecretKey1234567890",
  "Issuer": "AuthSystem",
  "Audience": "AuthSystemAPI"
}


Run the Application:
dotnet run --project AuthenticationSystem.API
