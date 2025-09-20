# 🏠 Rental / Property Management System - API

This is the **backend API** for the Rental / Property Management System, built with **C# ASP.NET Core Web API**.  
It provides all business logic, authentication, and database access for managing properties, tenants, leases, and payments.  

The API follows a **Repository Pattern** architecture to keep the codebase modular, testable, and maintainable.  
It exposes RESTful endpoints consumed by the **React frontend application**.  

👉 Frontend Repository: [Rental Management System - Frontend](https://github.com/MajorDev12/RentalManager_WebApp)  

---

## 🛠 Tech Stack

- **C# 12**
- **ASP.NET 8**
- **C# ASP.NET Core Web API**
- **Entity Framework Core** (ORM)
- **SQL Server** (database used)
- **Dependency Injection** (for services and repositories)
- **JWT Authentication** In Development

---


### Data Modeling

**Entity Framework Core Fluent API**.  
This approach ensures that all configurations (keys, constraints, relationships, and table mappings) are centralized and not tightly coupled to model classes via attributes.  

Entity configurations are placed under the `Configurations/` folder and applied in the `OnModelCreating` method of `ApplicationDbContext`.


---

## 📂 Project Structure (Repository Pattern)

```plaintext
RentalManagementAPI/
│
├── Controllers/         # Handle HTTP requests and responses
│   └── PropertyController.cs
│   └── TenantController.cs
│   └── TransactionController.cs
│
├── DTOs/                # Data Transfer Objects for requests/responses
│   └── PropertyDto.cs
│   └── TenantDto.cs
│
├── Data/                # DbContext and database initialization
│   └── ApplicationDbContext.cs
│
├── Helpers/             # Utility classes (e.g. password hashing, extensions)
│   └── JwtHelper.cs
│   └── ModelStateValidator.cs
│
├── Mappings/           
│   └── MappingProfile.cs
│
├── Models/              # Domain models/entities
│   └── Property.cs
│   └── Tenant.cs
│   └── Lease.cs
│
├── Repositories/        # Repository interfaces and implementations
│   └── IPropertyRepository.cs
│   └── PropertyRepository.cs
│
├── Services/            # Business logic layer
│   └── IPropertyService.cs
│   └── PropertyService.cs
│
├── Program.cs           # Application entry point
└── appsettings.json     # App configuration (DB connection string, JWT keys, etc.)
```





