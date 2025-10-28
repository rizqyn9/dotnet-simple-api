# 🧩 SampleApi — ASP.NET Core Clean API Boilerplate

> A clean, modular ASP.NET Core 8 Web API template with FluentValidation, JWT Auth, PostgreSQL, API Versioning, and CI/CD via GitHub Actions.

Built by **RizeForge** ⚙️ — a DevOps-driven engineering brand focusing on reliable, production-grade backend systems.

---

## 🚀 Features

- ✅ **Clean Architecture** — separation of `Controllers`, `Services`, `Repositories`, and `Data`
- 🧠 **FluentValidation** — model validation made easy and consistent
- 🔐 **JWT Authentication** — secure access control for users and admins
- 🧩 **API Versioning** — support for multiple versions (`v1`, `v2`)
- 💾 **PostgreSQL + EF Core** — robust data access with migrations
- 🧱 **Swagger / OpenAPI** — interactive API documentation per version
- 🐳 **Docker-ready** — production image with GHCR publishing
- ⚙️ **GitHub Actions CI/CD** — automatic Docker build & push to GHCR with semantic versioning

---

## 📂 Project Structure

```

SampleApi/
│
├── Controllers/
│ ├── AuthController.cs
│ ├── UserController.cs
│ └── UserControllerV2.cs
│
├── Data/
│ ├── ApplicationDbContext.cs
│ └── Migrations/ (optional if kept in root)
│
├── DTOs/
│ ├── CreateUserDto.cs
│ └── UserDto.cs
│
├── Middleware/
│ └── ErrorHandlingMiddleware.cs
│
├── Models/
│ ├── User.cs
│ ├── UserRole.cs
│ └── ApiResponse.cs
│
├── Repositories/
│ ├── IUserRepository.cs
│ └── UserRepository.cs
│
├── Services/
│ ├── IUserService.cs
│ ├── UserService.cs
│ └── AuthService.cs
│
├── Validators/
│ └── CreateUserDtoValidator.cs
│
├── Configurations/
│ └── SwaggerGenOptionsSetup.cs
│
├── Program.cs
├── Dockerfile
└── README.md

```

---

## ⚡ Quick Start (Local Development)

### 1️⃣ Clone the repo

```bash
git clone https://github.com/rizqyn9/dotnet-simple-api.git
cd dotnet-simple-api
```

### 2️⃣ Setup environment

Create a `.env` or `appsettings.Development.json` file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=sampledb;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "supersecretkey123",
    "Issuer": "SampleApi",
    "Audience": "SampleApiClient"
  }
}
```

### 3️⃣ Run the API

```bash
dotnet build
dotnet ef database update
dotnet run
```

Swagger will open automatically at:
👉 [http://localhost:5229](http://localhost:5229)

---

## 🐳 Docker Build & Run

```bash
docker build -t sampleapi:latest .
docker run -d -p 5229:8080 sampleapi:latest
```

Or pull from GHCR:

```bash
docker pull ghcr.io/rizqyn9/sampleapi:latest
docker run -d -p 5229:8080 ghcr.io/rizqyn9/sampleapi:latest
```

---

## 🔄 CI/CD — GitHub Actions

Every push to `main` or a tagged release (e.g. `v1.0.0`) triggers a build and publish to GitHub Container Registry (GHCR).

See: `.github/workflows/docker-build.yml`

### 🧠 Workflow Highlights

- **Automatic semantic tags**: `latest`, `main`, `v1.0.0`, `sha-abc123`
- **OCI Metadata labels** for traceability
- **Multi-branch support** (e.g. staging or release branches)

---

## 🧪 API Versions

| Version | Endpoint        | Description                                   |
| ------- | --------------- | --------------------------------------------- |
| `v1`    | `/api/v1/users` | Original implementation                       |
| `v2`    | `/api/v2/users` | Extended version with `me` and `role` support |

To view documentation for each version:

```
http://localhost:5229/swagger/v1/swagger.json
http://localhost:5229/swagger/v2/swagger.json
```

---

## 🧱 Technology Stack

| Layer          | Tech                               |
| -------------- | ---------------------------------- |
| **Framework**  | ASP.NET Core 8                     |
| **ORM**        | Entity Framework Core (PostgreSQL) |
| **Auth**       | JWT Bearer                         |
| **Validation** | FluentValidation                   |
| **Docs**       | Swagger / Swashbuckle              |
| **CI/CD**      | GitHub Actions + GHCR              |
| **Container**  | Docker (Buildx)                    |

---

## 🧰 Commands

| Task             | Command                                           |
| ---------------- | ------------------------------------------------- |
| Build            | `dotnet build`                                    |
| Run              | `dotnet run`                                      |
| Apply migrations | `dotnet ef database update`                       |
| Create migration | `dotnet ef migrations add <Name>`                 |
| Docker build     | `docker build -t sampleapi:latest .`              |
| Push to GHCR     | `docker push ghcr.io/<username>/sampleapi:latest` |

---

## 🧠 About RizeForge

> RizeForge is a DevOps & Engineering brand focused on automation, scalability, and developer experience.
> Follow along for clean, production-ready backend templates.
