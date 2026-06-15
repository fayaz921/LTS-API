# Litigation Tracking System — API (LTS-API)

A multi-tenant SaaS platform built for Pakistani law firms and legal departments. LTS digitizes case management, hearing tracking, document storage, and automated alerts — replacing manual paper-based workflows.

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Features](#features)
- [Getting Started](#getting-started)
- [Environment Variables](#environment-variables)
- [API Documentation](#api-documentation)
- [Database](#database)
- [Background Jobs](#background-jobs)
- [Authentication](#authentication)
- [Multi-Tenancy](#multi-tenancy)
- [Contributing](#contributing)

---

## Overview

LTS is a multi-tenant SaaS application where each law firm (organization) operates in complete isolation. The system supports:

- **SuperAdmin** — manages all organizations, plans, and subscriptions across the platform
- **Admin** — manages their organization's users, cases, and settings
- **User** — manages cases, hearings, documents, and petitioners within their organization

---

## Tech Stack

| Category | Technology |
|----------|-----------|
| Framework | ASP.NET 10 Web API |
| Language | C# |
| ORM | Entity Framework Core |
| Database | PostgreSQL (Neon) |
| Architecture | Vertical Slice + CQRS + MediatR |
| Authentication | JWT Bearer Tokens |
| Password Hashing | `IPasswordHasher` (ASP.NET Identity) |
| File Storage | Cloudinary |
| Background Jobs | Hangfire |
| Logging | Serilog |
| Validation | FluentValidation |
| API Documentation | Scalar |
| Email | MailKit |

---

## Architecture

LTS follows **Vertical Slice Architecture** combined with **CQRS** and **MediatR**.

### Core Principles

- Code is organized by **feature**, not by layer
- Every feature is a **self-contained slice** — its own commands, queries, handlers, validators, DTOs, mappings, and controller
- **CQRS** separates reading from writing — Commands change data, Queries read data, they never mix
- **MediatR** acts as the dispatcher — controllers send a Command or Query to MediatR, MediatR finds the correct handler and runs it
- **DbContext is used directly** inside handlers — no repository pattern
- **Extension methods** handle all entity-to-DTO and DTO-to-entity mappings — no AutoMapper
- **No class libraries** — single LTS.API project

### Request Flow

```
React Frontend
      ↓
POST /api/Feature/ActionName
      ↓
Controller → _mediator.Send(command)
      ↓
MediatR Pipeline
      ↓
ValidationBehavior (FluentValidation) → 400 if invalid
      ↓
LoggingBehavior (Serilog)
      ↓
Handler → AppDbContext → PostgreSQL
      ↓
Response → ApiResponse<T> { Success, Data, Message }
      ↓
React Frontend
```

### API Route Convention

Routes follow `/api/Feature/ActionName` convention — not RESTful lowercase:

```
POST   /api/Cases/CreateCase
GET    /api/Cases/GetAllCases
GET    /api/Cases/GetCaseById/{id}
PUT    /api/Cases/UpdateCase
DELETE /api/Cases/DeleteCase/{id}
```

---

## Project Structure

```
LTS.API/
├── Features/
│   ├── Cases/
│   │   ├── Commands/
│   │   │   ├── CreateCase/
│   │   │   │   ├── CreateCaseCommand.cs
│   │   │   │   ├── CreateCaseHandler.cs
│   │   │   │   └── CreateCaseValidator.cs
│   │   │   ├── UpdateCase/
│   │   │   └── DeleteCase/
│   │   ├── Queries/
│   │   │   ├── GetAllCases/
│   │   │   ├── GetCaseById/
│   │   │   └── SearchCases/
│   │   ├── Mappings/
│   │   │   └── CaseMappings.cs
│   │   └── CasesController.cs
│   ├── Courts/
│   ├── Departments/
│   ├── Petitioners/
│   ├── Followup/
│   ├── Documents/
│   ├── Bench/
│   ├── Auth/
│   ├── Alerts/
│   ├── Reports/
│   └── SuperAdmin/
│
├── Domain/
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   ├── Case.cs
│   │   ├── Court.cs
│   │   ├── Department.cs
│   │   ├── Petitioner.cs
│   │   ├── CasePetitioner.cs
│   │   ├── Followup.cs
│   │   ├── Bench.cs
│   │   ├── CaseDocument.cs
│   │   └── User.cs
│   └── Enums/
│       ├── CaseStatus.cs
│       └── UserRole.cs
│
├── Infrastructure/
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── Migrations/
│   │   └── Configurations/
│   ├── Services/
│   │   ├── Interfaces/
│   │   │   ├── IEmailService.cs
│   │   │   └── IFileStorageService.cs
│   │   ├── EmailService.cs
│   │   └── FileStorageService.cs
│   └── BackgroundJobs/
│       └── HearingAlertJob.cs
│
├── Common/
│   ├── Behaviors/
│   │   ├── ValidationBehavior.cs
│   │   └── LoggingBehavior.cs
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Response/
│   │   └── ApiResponse.cs
│   └── Exceptions/
│       ├── NotFoundException.cs
│       └── UnauthorizedException.cs
│
├── appsettings.json
├── appsettings.Development.json
└── Program.cs
```

---

## Features

| Feature | Description |
|---------|-------------|
| **Auth** | Register, Login, JWT token issuance, OTP email verification, password reset |
| **Cases** | Full CRUD, case search, status management, email list per case |
| **Courts** | Court management with address and contact details |
| **Departments** | Department management linked to organizations |
| **Petitioners** | Petitioner profiles linked to cases (many-to-many) |
| **Followup / Hearings** | Hearing dates, interim orders, decisions, next hearing tracking |
| **Documents** | File upload/download via Cloudinary, linked to cases |
| **Bench** | Judge assignment per case with contact details |
| **Alerts** | Automated hearing alert emails 3 days before hearing date (Hangfire) |
| **Reports** | Department-wise and court-wise case summary reports |
| **SuperAdmin** | Organization management, plan assignment, subscription control |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) or a [Neon](https://neon.tech) account
- [Cloudinary](https://cloudinary.com) account
- [Git](https://git-scm.com/)

### Clone the Repository

```bash
git clone https://github.com/fayaz921/LTS-API.git
cd LTS-API
```

### Install Dependencies

```bash
dotnet restore
```

### Configure Environment Variables

Copy the example settings and fill in your values (see [Environment Variables](#environment-variables)):

```bash
# Update appsettings.Development.json with your values
```

### Run Migrations

```bash
dotnet ef database update
```

### Run the Project

```bash
dotnet run
```

API will be available at `https://localhost:7001`
Scalar documentation at `https://localhost:7001/scalar`

---

## Environment Variables

Configure the following in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=lts;Username=...;Password=..."
  },
  "JwtSettings": {
    "Secret": "your-secret-key-min-32-chars",
    "Issuer": "LTS-API",
    "Audience": "LTS-Client",
    "ExpiryInMinutes": 60
  },
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "FromName": "LTS System"
  },
  "SuperAdmin": {
    "Email": "superadmin@lts.com",
    "Password": "your-superadmin-password"
  }
}
```

> **Never commit `appsettings.Development.json` to version control.** It is already in `.gitignore`.

---

## API Documentation

API documentation is available via **Scalar** (not Swagger):

```
https://localhost:7001/scalar
```

All endpoints are grouped by feature and fully documented with request/response schemas.

---

## Database

- **Provider:** PostgreSQL via Neon (serverless)
- **ORM:** Entity Framework Core with code-first migrations
- **Configurations:** Each entity has its own `IEntityTypeConfiguration` class under `Infrastructure/Persistence/Configurations/`

### Run a New Migration

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Key Indexes

The following columns are indexed for query performance:

- `OrganizationId` — on every entity (multi-tenancy filtering)
- `Status` — on Cases
- `DateInstitution` — on Cases
- `HearingDate` — on Followup

---

## Background Jobs

Hangfire is used for background job scheduling:

| Job | Schedule | Description |
|-----|----------|-------------|
| `HearingAlertJob` | Daily | Sends email alerts for hearings within the next 3 days |
| `SubscriptionExpiryJob` | Daily | Sends email when organization subscription is about to expire |

Hangfire dashboard is available at `/hangfire` (SuperAdmin only).

---

## Authentication

- JWT Bearer tokens issued on login
- Tokens include claims: `UserId`, `OrganizationId`, `Role`
- All protected endpoints require `Authorization: Bearer <token>` header
- Roles: `SuperAdmin`, `Admin`, `User`
- Passwords hashed using ASP.NET `IPasswordHasher`

---

## Multi-Tenancy

LTS is a **multi-tenant SaaS** — each organization's data is fully isolated:

- Every entity carries an `OrganizationId` foreign key
- `CurrentUserService` extracts `OrganizationId` from the JWT token on every request
- Every handler filters queries by `OrganizationId` — no cross-organization data leakage
- SuperAdmin role bypasses tenant filtering to manage all organizations

---

## Contributing

This is a private team project. Branch and PR rules:

- **Never push directly to `main`**
- Create a feature branch: `Feature/YourFeatureName`
- Open a Pull Request — CI must pass before merge
- PR must be reviewed and approved by the Tech Lead before merging

### Branch Naming Convention

```
Feature/CreateCase
Feature/UpdateCourt
Feature/AddReports
```

### Golden Rules

- Entities live in `Domain/Entities` only — never inside Features
- DTOs and Commands live inside their feature slice only — never shared between features
- Use `AppDbContext` directly inside handlers — no repositories
- Use extension methods in `Mappings/` for all mapping — no AutoMapper
- Controllers must be thin — one line only: `_mediator.Send()`
- Every Command must have a FluentValidation Validator
- Always use `AsNoTracking()` on GET queries
- Always filter by `OrganizationId` on every query

---

## Related Repositories

| Repo | Description |
|------|-------------|
| [LTS-Client](https://github.com/fayaz921/LTS-Client) | React + TypeScript frontend dashboard |
| LTS-Landing | Next.js landing page (coming soon) |

---

*Litigation Tracking System — Built for Pakistani Law Firms*
