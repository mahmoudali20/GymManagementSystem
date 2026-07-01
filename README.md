# Gym Management System

A modern, scalable gym management application built with **.NET 8** and **Razor Pages**, designed to streamline gym operations, member management, and fitness tracking.

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Architecture](#project-architecture)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [API & Services](#api--services)
- [Database](#database)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

Gym Management System is a comprehensive solution for managing gym operations including:
- **Member Management**: Track member information, memberships, and health records
- **Training Plans**: Create and manage fitness plans for members
- **Session Management**: Schedule and track training sessions
- **Trainer Management**: Manage trainer profiles and assignments
- **Membership Tracking**: Handle membership plans and renewals

The application follows **clean architecture** principles with clear separation of concerns across presentation, business logic, and data access layers.

## ✨ Features

### Core Functionality
- ✅ **Member Management**
  - Create, update, and delete member profiles
  - Track health records and fitness metrics
  - Manage member contact information

- ✅ **Membership Plans**
  - Create and manage membership plans
  - Track membership status and expiration
  - Handle membership renewals

- ✅ **Training Sessions**
  - Schedule training sessions
  - Assign trainers to sessions
  - Track session attendance

- ✅ **Fitness Plans**
  - Design personalized training plans
  - Track plan progress
  - Manage plan assignments

- ✅ **Trainer Management**
  - Maintain trainer profiles
  - Track trainer assignments
  - Manage trainer specializations

### Technical Features
- 🔐 Secure data persistence with SQL Server
- 🗄️ Entity Framework Core for database operations
- 🔄 Repository Pattern for data access
- 📊 Unit of Work pattern for transaction management
- 🎨 AutoMapper for DTO mappings
- ⚡ Async/await patterns for performance
- 🛡️ Validation and error handling

## 🛠️ Tech Stack

| Component | Technology |
|-----------|-----------|
| **Framework** | .NET 8 |
| **Web Framework** | ASP.NET Core with Razor Pages |
| **Database** | SQL Server |
| **ORM** | Entity Framework Core |
| **Object Mapping** | AutoMapper |
| **Architecture** | Clean Architecture (3-Layer) |
| **Language** | C# |

## 🏗️ Project Architecture

The solution follows a **3-Tier Architecture** pattern:

```
GymManagement.PL (Presentation Layer)
├── Pages (Razor Pages)
├── ViewModels
├── Controllers
└── Program.cs (Configuration)

GymManagement.BLL (Business Logic Layer)
├── Services
├── Interfaces
├── ViewModels
├── Common (Utilities)
└── Profiles (AutoMapper)

GymManagement.DAL (Data Access Layer)
├── Context (DbContext)
├── Models (Entities)
├── Repositories
├── Seeds (Initial Data)
└── Migrations
```

### Layer Responsibilities

- **PL (Presentation Layer)**: User interface and request handling
- **BLL (Business Logic Layer)**: Business rules, validation, and service implementation
- **DAL (Data Access Layer)**: Database operations and data persistence

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK or later
- SQL Server (2019 or later)
- Visual Studio 2026 (Community, Professional, or Enterprise)
- Git

### Installation

1. **Clone the repository**
   ```powershell
   git clone https://github.com/mahmoudali20/GymManagementSystem.git
   cd GymManagementSystem
   ```

2. **Restore NuGet packages**
   ```powershell
   dotnet restore
   ```

3. **Update the database**
   ```powershell
   dotnet ef database update
   ```

4. **Build the solution**
   ```powershell
   dotnet build
   ```

5. **Run the application**
   ```powershell
   dotnet run --project GymManagement.PL
   ```

The application will be available at `https://localhost:5001` (or the configured port).

## ⚙️ Configuration

### appsettings.json

Update the connection string and other settings in `GymManagement.PL/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=GymManagementDb;Trusted_Connection=true;Encrypt=false;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Database Configuration

The application uses Entity Framework Core with SQL Server. Migrations are automatically applied on startup.

### AutoMapper Configuration

AutoMapper profiles are configured in `GymManagement.BLL/Profiles/MappingProfile.cs` to handle DTO conversions.

## 📁 Project Structure

### GymManagement.DAL
- **Models**: Core entity models (Member, Trainer, MemberShip, Session, Plan, etc.)
- **Context**: `GymDbContext` for Entity Framework configuration
- **Repositories**: Generic and specialized repositories for data access
- **Seeds**: Initial data seeding
- **Migrations**: Database schema versions

### GymManagement.BLL
- **Services**: Business logic implementation
  - `MemberService`: Member operations
  - `MemberShipService`: Membership management
  - `SessionService`: Session management
  - `PlanService`: Plan management
  - `TrainerService`: Trainer operations
- **Interfaces**: Service contracts
- **ViewModels**: DTOs for UI and API
- **Common**: Shared utilities and Result types
- **Profiles**: AutoMapper configurations

### GymManagement.PL
- **Pages**: Razor Pages for UI
- **Controllers**: MVC controllers (if applicable)
- **ViewModels**: Page-specific models
- **wwwroot**: Static files (CSS, JS, images)
- **Program.cs**: Dependency injection and middleware configuration

## 🔌 API & Services

### MemberService
- `createMemberAsync()`: Create new member with validation
- Member email and phone uniqueness validation
- Health record tracking

### MemberShipService
- Manage membership plans and subscriptions
- Track membership status

### SessionService
- Schedule and manage training sessions
- Assign trainers to sessions

### PlanService
- Create and manage fitness plans
- Track plan assignments

### TrainerService
- Manage trainer profiles
- Track trainer assignments

### Result Pattern
All services return a `Result` object for consistent error handling:
```csharp
public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public ResultKind Kind { get; set; }
}
```

## 🗄️ Database

### Entity Framework Configuration
- Uses SQL Server as the database provider
- Supports async operations for better performance
- Implements Repository Pattern for data access
- Uses Unit of Work pattern for managing transactions

### Core Entities
- **Member**: Gym member information
- **Trainer**: Trainer profiles
- **MemberShip**: Membership subscriptions
- **Session**: Training sessions
- **Plan**: Fitness training plans
- **HealthRecord**: Member health metrics

### Migrations
Generate migrations with:
```powershell
dotnet ef migrations add MigrationName --project GymManagement.DAL
dotnet ef database update
```

## 📝 Development Guidelines

### Adding a New Service
1. Create interface in `GymManagement.BLL/Interfaces/INewService.cs`
2. Implement service in `GymManagement.BLL/Classes/NewService.cs`
3. Register in `GymManagement.PL/Program.cs`
4. Create ViewModels in `GymManagement.BLL/ViewModels/`
5. Add AutoMapper profiles in `GymManagement.BLL/Profiles/MappingProfile.cs`

### Database Changes
1. Create entity in `GymManagement.DAL/Models/`
2. Add DbSet to `GymDbContext`
3. Create migration: `dotnet ef migrations add AddNewEntity --project GymManagement.DAL`
4. Update database: `dotnet ef database update`

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is open source and available under the MIT License - see the LICENSE file for details.

## 📧 Contact & Support

For questions, issues, or suggestions, please:
- Open an issue on [GitHub Issues](https://github.com/mahmoudali20/GymManagementSystem/issues)
- Contact the project maintainer at [your-email@example.com]

---

**Made with Mahmoud Ali ❤️ for fitness enthusiasts and gym managers**

*Last Updated: 2026*