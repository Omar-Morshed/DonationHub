# DonationHub — Backend API (Sprint 1)

**DonationHub** is a Donation Management & Crowdfunding Platform built with **.NET 10**, following **Clean Architecture** principles, **Vertical Slice** feature organization, and a **Rich Domain Model**.

Sprint 1 establishes the architectural foundation of the platform, EF Core persistence with interceptors, and two core production-ready modules: **Identity & Authentication** and **Categories Management**.

---

## 🛠️ Tech Stack & Prerequisites

### Prerequisites

- **.NET 10 SDK** (`net10.0`)
- **SQL Server** (SQL Server Express, LocalDB, or Docker container)
- **EF Core CLI Tools** (`dotnet tool install --global dotnet-ef`)

### Key Technologies

- **Framework:** ASP.NET Core 10 Web API (`.slnx` solution format)
- **ORM & Database:** Entity Framework Core 10 & SQL Server
- **Authentication & Security:** ASP.NET Core Identity (`Guid` keys), JWT Bearer Authentication, SHA-256 Hashed Refresh Tokens with Rotation
- **API Documentation:** Built-in OpenAPI + **Scalar** (`Scalar.AspNetCore`)

---

## 🏗️ Architecture & Project Structure

The solution enforces strict unidirectional dependencies across four layers:

```text
DonationHub.slnx
├── src/
│   ├── DonationHub.Domain/          # Core entities, Value Objects, enums, domain invariants (Zero external dependencies)
│   ├── DonationHub.Application/     # Vertical Slices (Features), DTOs, interfaces (IGenericRepository, IUnitOfWork), Options
│   ├── DonationHub.Infrastructure/  # AppDbContext, Fluent API configs, Interceptors, Identity, Seeders, Repositories
│   └── DonationHub.Api/             # Thin Controllers, Middlewares, Scalar/OpenAPI setup, Composition Root (Program.cs)
```

- **Domain:** Pure C# rich domain models with private setters, factory methods, and encapsulated business invariants.
- **Application:** Organized by **Vertical Slices** (`Features/Auth`, `Features/Categories`) rather than flat technical folders.
- **Infrastructure:** Implements persistence, ASP.NET Core Identity, startup data seeders, and `SoftDeleteInterceptor`.
- **Api:** Serves as the presentation layer and composition root (`/api/v1` base route).

---

## 🚀 Getting Started (Setup & Run Guide)

### 1. Clone the Repository

```bash
git clone https://github.com/Omar-Morshed/DonationHub
cd DonationHub
```

### 2. Configure Secrets (`dotnet user-secrets`)

> ⚠️ **Security Note:** Never commit real passwords, connection strings, or JWT signing keys to `appsettings.json`. All sensitive values are validated at startup via `ValidateOnStart()`.

Initialize and configure user secrets inside the **API project** directory:

```bash
cd src/DonationHub.Api
dotnet user-secrets init

# 1. Database Connection String
dotnet user-secrets set "ConnectionStrings:DefaultConnectionString" "Server=localhost;Database=DonationHubDb;Trusted_Connection=True;TrustServerCertificate=True;"

# 2. JWT Configuration (SecretKey must be at least 32 characters)
dotnet user-secrets set "JwtOptions:SecretKey" "YourSuperSecretSigningKeyWithAtLeast32Chars!"
dotnet user-secrets set "JwtOptions:Issuer" "DonationHub.Api"
dotnet user-secrets set "JwtOptions:Audience" "DonationHub.Client"

# 3. Initial Admin Account (Seeded in all environments)
dotnet user-secrets set "SeedAdminOptions:Email" "admin@donationhub.com"
dotnet user-secrets set "SeedAdminOptions:Password" "Admin@1234"
dotnet user-secrets set "SeedAdminOptions:FullName" "System Administrator"

# 4. Development Test Users (Seeded in Development environment only)
dotnet user-secrets set "DevSeedOptions:CampaignManagerEmail" "manager@donationhub.com"
dotnet user-secrets set "DevSeedOptions:CampaignManagerPassword" "Manager@1234"
dotnet user-secrets set "DevSeedOptions:DonorEmail" "donor@donationhub.com"
dotnet user-secrets set "DevSeedOptions:DonorPassword" "Donor@1234"
```

### 3. Apply Database Migrations

Run the following command from the solution root to create the database and apply all EF Core migrations:

```bash
dotnet ef database update --project src/DonationHub.Infrastructure --startup-project src/DonationHub.Api
```

### 4. Run the Application & Automatic Data Seeding

Start the API project:

```bash
dotnet run --project src/DonationHub.Api
```

Upon startup, the database seeder automatically runs before accepting HTTP requests:

- **All Environments:** Seeds the three system roles (`Admin`, `CampaignManager`, `Donor`) and the primary `Admin` account configured in `SeedAdminOptions`.
- **Development Environment Only:** Seeds two demo accounts (`CampaignManager` and `Donor`) and five bilingual categories (_Health_, _Education_, _Relief_, _Orphans_, _Environment_).

---

## 📖 Interactive API Documentation (Scalar)

Once the application is running, open your browser and navigate to:

- **Scalar UI:** `https://localhost:<port>/scalar/v1`

### Testing Protected Endpoints

1. Execute `POST /api/v1/auth/login` using the seeded Admin credentials (`admin@donationhub.com` / `Admin@1234`).
2. Copy the `accessToken` from the JSON response.
3. Click the **Auth / Authorize** button in Scalar, select **Bearer**, paste your token, and test protected endpoints such as `POST /api/v1/categories` or `POST /api/v1/admin/staff`.

---

## 🔌 API Endpoints Overview (Sprint 1)

### Identity & Authentication (`/api/v1`)

| Method | Endpoint                | Access        | Description                                                      |
| :----- | :---------------------- | :------------ | :--------------------------------------------------------------- |
| `POST` | `/api/v1/auth/register` | Anonymous     | Registers a new user with the `Donor` role.                      |
| `POST` | `/api/v1/auth/login`    | Anonymous     | Authenticates user and returns JWT Access Token + Refresh Token. |
| `POST` | `/api/v1/auth/refresh`  | Anonymous     | Rotates refresh token (revokes old token & issues a new pair).   |
| `POST` | `/api/v1/auth/logout`   | Authenticated | Revokes the active refresh token.                                |
| `GET`  | `/api/v1/auth/me`       | Authenticated | Returns the current authenticated user's profile.                |
| `POST` | `/api/v1/admin/staff`   | `Admin` Only  | Creates an `Admin` or `CampaignManager` account.                 |

### Categories Module (`/api/v1`)

| Method   | Endpoint                  | Access       | Description                                                         |
| :------- | :------------------------ | :----------- | :------------------------------------------------------------------ |
| `GET`    | `/api/v1/categories`      | Anonymous    | Lists all active bilingual categories.                              |
| `GET`    | `/api/v1/categories/{id}` | Anonymous    | Retrieves a single category by ID.                                  |
| `POST`   | `/api/v1/categories`      | `Admin` Only | Creates a new bilingual category (unique `NameEn` & `NameAr`).      |
| `PUT`    | `/api/v1/categories/{id}` | `Admin` Only | Updates an existing category.                                       |
| `DELETE` | `/api/v1/categories/{id}` | `Admin` Only | Deletes a category (returns `409 Conflict` if linked to campaigns). |

---

## 🧠 Architectural & Design Decisions

### 1. Read/Write Separation (Generic Repository + UoW vs. Direct `DbContext`)

- **Write Operations (Commands):** All state-mutating operations (`Register`, `RefreshToken` rotation, `Create/Update/Delete` Categories) go through `IGenericRepository<T>` and `IUnitOfWork` with a single `SaveChangesAsync()` call. This ensures domain invariants are enforced on tracked entities and guarantees atomic database transactions.
- **Read Operations (Queries):** Read endpoints (`GET /categories`, `GET /auth/me`) query `AppDbContext` directly using `.AsNoTracking()` and project straight into DTOs via `.Select(...)`. This avoids unnecessary EF Core Change Tracker memory overhead and fetches only the required columns from SQL Server.

### 2. Identity & Domain Table Splitting

To keep `DonationHub.Domain` completely free of external dependencies (`Microsoft.AspNetCore.Identity`), security concerns (`ApplicationUser : IdentityUser<Guid>`) live in `Infrastructure` while business properties (`Domain.User`) live in `Domain`. Both entities are mapped to the **same database table (`Users`)** via EF Core **Table Splitting** using a shared `Guid` Primary Key.

### 3. Cryptographically Secure & Hashed Refresh Tokens

Refresh tokens are generated using `RandomNumberGenerator.GetBytes(32)` (256-bit CSPRNG) and stored in the database strictly as **SHA-256 hashes**. On every refresh request, the old token is marked as revoked (`RevokedAt`) and linked to its replacement (`ReplacedByTokenId`) to prevent replay attacks.

### 4. Interceptor-Driven Soft Delete

Entities implementing `ISoftDeletable` are never hard-deleted. `SoftDeleteInterceptor` intercepts `EntityState.Deleted` during `SaveChangesAsync`, converts it to `EntityState.Modified`, and sets `IsDeleted = true` and `DeletedAtUtc`. Global query filters (`HasQueryFilter(e => !e.IsDeleted)`) automatically exclude soft-deleted records from standard queries.

### 5. Fail-Fast Configuration Validation

All configuration sections (`JwtOptions`, `SeedAdminOptions`, `DonationOptions`) use strongly typed classes decorated with Data Annotations and registered with `.ValidateOnStart()`. If any required secret or setting is missing or invalid, the application fails immediately at startup rather than at runtime.
