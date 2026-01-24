# 💰 Expense Manager

Personal expense management system with Clean Architecture.

## 🚀 Tech Stack

**Backend:**
- .NET 9 / C# 13
- Entity Framework Core
- SQL Server (Docker)
- xUnit + FluentAssertions

**Frontend (Coming Soon):**
- React + TypeScript

## 🏗️ Architecture

- Clean Architecture
- Domain-Driven Design (DDD)
- Repository Pattern
- SOLID Principles

## ✅ Current Progress

- [x] Domain Entities (User, Category)
- [x] Value Objects (Email, Money, HashedPassword)
- [x] 78+ Unit Tests
- [x] REST API with Swagger
- [ ] Expense Entity (WIP)
- [ ] Database Integration
- [ ] Authentication (JWT)
- [ ] Frontend

## 🏃 How to Run
```bash
# 1. Start SQL Server
docker-compose up -d

# 2. Run API
cd src/ExpenseManager.API
dotnet run

# 3. Run Tests
cd src/ExpenseManager.Tests
dotnet test
```

Open Swagger: `https://localhost:7XXX/swagger`

## 📁 Project Structure
```
ExpenseManager/
├── src/
│   ├── ExpenseManager.API/          # Controllers, DTOs
│   ├── ExpenseManager.Application/  # Use Cases (Future)
│   ├── ExpenseManager.Domain/       # Entities, Value Objects
│   ├── ExpenseManager.Infrastructure/ # Database (Future)
│   └── ExpenseManager.Tests/        # Unit Tests
└── docker-compose.yml
```

## 👨‍💻 Learning Journey

This is a learning project focused on:
- Clean Architecture best practices
- Test-Driven Development (TDD)
- Domain-Driven Design concepts
- .NET modern features
