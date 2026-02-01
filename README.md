# 💰 Expense Manager

Personal expense management system built with .NET 9 and React.

## 🚀 Tech Stack

### Backend
- .NET 9 / C# 13
- Entity Framework Core 9
- SQL Server 2022 (Docker)
- xUnit + FluentAssertions (Tests)
- Swashbuckle (Swagger)

### Frontend (Coming Soon)
- React
- TypeScript

## 🏗️ Architecture

- **Clean Architecture**
- **Domain-Driven Design (DDD)**
- **Repository Pattern**
- **SOLID Principles**
- **Unit Tests**

## 📁 Project Structure
```
ExpenseManager/
├── src/
│   ├── ExpenseManager.API/
│   │   ├── Controllers/
│   │   │   ├── UsersController.cs
│   │   │   ├── CategoriesController.cs
│   │   │   └── ExpensesController.cs
│   │   ├── DTOs/
│   │   │   ├── Users/Requests/
│   │   │   ├── Categories/Requests/
│   │   │   └── Expenses/Requests/
│   │   └── Program.cs
│   ├── ExpenseManager.Application/
│   │   └── Interfaces/
│   │       ├── IUserRepository.cs
│   │       ├── ICategoryRepository.cs
│   │       └── IExpenseRepository.cs
│   ├── ExpenseManager.Domain/
│   │   ├── Common/
│   │   │   └── Entity.cs
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── Category.cs
│   │   │   └── Expense.cs
│   │   ├── Enums/
│   │   │   ├── ExpenseType.cs
│   │   │   └── PaymentMethod.cs
│   │   └── ValueObjects/
│   │       ├── Email.cs
│   │       ├── Money.cs
│   │       └── HashedPassword.cs
│   ├── ExpenseManager.Infrastructure/
│   │   ├── Data/
│   │   │   ├── Configurations/
│   │   │   │   ├── UserConfiguration.cs
│   │   │   │   ├── CategoryConfiguration.cs
│   │   │   │   └── ExpenseConfiguration.cs
│   │   │   └── ExpenseManagerDbContext.cs
│   │   └── Repositories/
│   │       ├── UserRepository.cs
│   │       ├── CategoryRepository.cs
│   │       └── ExpenseRepository.cs
│   └── ExpenseManager.Tests/
│       └── Domain/
│           ├── Entities/
│           │   ├── UserTests.cs
│           │   ├── CategoryTests.cs
│           │   └── ExpenseTests.cs
│           └── ValueObjects/
│               ├── EmailTests.cs
│               ├── MoneyTests.cs
│               └── HashedPasswordTests.cs
└── docker-compose.yml
```

## ✅ Features

- [x] User entity with validations
- [x] Category entity with validations
- [x] Expense entity with validations
- [x] Value Objects (Email, Money, HashedPassword)
- [x] Enums (ExpenseType, PaymentMethod)
- [x] Entity Framework Core + SQL Server
- [x] Repository Pattern (interfaces + implementations)
- [x] REST API with Swagger documentation
- [x] 120+ Unit Tests
- [ ] Authentication (JWT)
- [ ] Pagination and filters
- [ ] Dashboard and reports
- [ ] Frontend (React)
- [ ] Deploy

## 🏃 How to Run

### Prerequisites
- .NET 9 SDK
- Docker Desktop

### Steps

1. Start SQL Server:
```bash
docker-compose up -d
```

2. Run API:
```bash
cd src/ExpenseManager.API
dotnet run
```

3. Open Swagger:
```
https://localhost:7XXX/swagger
```

### Run Tests
```bash
cd src/ExpenseManager.Tests
dotnet test
```

## 🐳 Docker

### Start database:
```bash
docker-compose up -d
```

### Stop database:
```bash
docker-compose down
```

### Check status:
```bash
docker ps
```

## 🗄️ Database

- **Server:** localhost,1433
- **Database:** ExpenseManagerDb
- **User:** sa

## 👨‍💻 Learning Journey

This is a learning project focused on:
- Clean Architecture best practices
- Test-Driven Development (TDD)
- Domain-Driven Design concepts
- .NET modern features

## 👨‍💻 Author

Walid Alsaid Daoud