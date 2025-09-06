# Yerbowo

## Overview
Yerbowo is an online store for Yerba-mate, teas, herbs, and accessories. The platform features a modern Angular frontend and a robust .NET 8 backend, delivering a seamless e-commerce experience.

---

## Technologies

**Frontend:**
- Angular 16
- RxJS, ngx-bootstrap, AlertifyJS, Auth0 Angular JWT
- Node.js 20, npm

**Backend:**
- .NET 8 (ASP.NET Core)
- C#, Serilog, NWebsec, Azure Key Vault integration
- SQL Server 2019 (Docker container)

**DevOps & CI/CD:**
- Docker & Docker Compose
- Azure DevOps pipelines (CI/CD)
- Azure Web App & Azure Container Apps

---

## Repository Structure

```
Yerbowo/
├─ YerbowoBackend/
│  ├─ src/
│  │  ├─ Yerbowo.Api/           # API endpoints, controllers
│  │  ├─ Yerbowo.Application/   # Orchestrates use cases, CQRS pattern etc.
│  │  ├─ Yerbowo.Domain/        # Domain entities, domain events, aggregates etc.
│  │  └─ Yerbowo.Infrastructure/# Data access, external services
│  └─ tests/
│     ├─ Yerbowo.Unit.Tests/        # Unit tests (xUnit, Moq, FluentAssertions)
│     └─ Yerbowo.Integration.Tests/ # Integration tests (API/WebApplicationFactory)
├─ YerbowoFrontend/
│  ├─ src/app/                   # Angular modules & components
│  └─ e2e/                       # End-to-end tests
├─ docker-compose.yml
├─ Dockerfile
├─ env.template                  # Environment variables template
├─ ci-build.yml                  # CI pipeline
├─ cd-deploy.yml                 # CD pipeline
```

---

## Setup and Installation

### Prerequisites
- Docker & Docker Compose
- Node.js & Angular CLI
- .NET 8 SDK
- Optional: Azure subscription for deployment

### Environment Variables
Create `.env` from `env.template`:

```env
ASPNETCORE_ENVIRONMENT=
ANGULAR_ENVIRONMENT=
ASPNETCORE_URLS=
ConnectionStrings__DefaultConnection=
SA_PASSWORD=
AZURE_KEYVAULT_URL=
AZURE_CLIENT_ID=
AZURE_CLIENT_SECRET=
AZURE_TENANT_ID=
```

### Local Development with Docker
Start backend, frontend, and database in containers:

```bash
docker-compose up --build
```

- Frontend: `http://localhost:8888`
- SQL Server: `localhost:1433`

Stop containers:

```bash
docker-compose down
```

### Frontend (Manual)
```bash
cd YerbowoFrontend
npm install
ng serve --ssl
```

### Backend (Manual)
```bash
cd YerbowoBackend
dotnet restore
dotnet build
dotnet run
```

---

## Testing

### Backend
Run unit & integration tests:

```bash
dotnet test Yerbowo.Unit.Tests/Yerbowo.Unit.Tests.csproj
dotnet test Yerbowo.Integration.Tests/Yerbowo.Integration.Tests.csproj
```

### Frontend
Run Angular tests:

```bash
ng test
ng e2e
```

---

## CI/CD Pipelines (Azure DevOps)

- **CI Pipeline (`ci-build.yml`)**: Builds frontend & backend, runs tests, generates coverage reports.
- **CD Pipeline (`cd-deploy.yml`)**: Deploys to Azure Web App or Azure Container App.
- Artifacts from CI are combined using `combine-artifacts.yml` (Angular build copied into backend wwwroot).

**Key features:**
- Automated backend & frontend build
- Unit, integration, and e2e tests
- Code coverage reports
- Deployment to Azure

---

## Docker Build (Multi-stage)

Dockerfile performs:

1. Angular production build
2. .NET backend restore, build, publish
3. Runtime image with Angular assets in `wwwroot`
   - Exposes port 80 for ASP.NET Core

---

## License
This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

**MIT License**

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED.