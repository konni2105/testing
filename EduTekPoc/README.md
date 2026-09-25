# EduTek POC

A full-stack educational management platform built with **ASP.NET Core 10**, demonstrating:
clean architecture, JWT authentication with refresh token rotation, role-based access control,
in-memory subject caching, and a comprehensive integration test suite.

---

## Project Structure

```
EduTekPoc/
├── EduTek.API/              # Web API (Swagger/OpenAPI, JWT, middleware)
├── EduTek.Application/      # Use-cases: services, DTOs, interfaces
├── EduTek.Infrastructure/   # EF Core DbContext, repositories, migrations
├── EduTek.Web/              # ASP.NET Core MVC frontend (session auth)
├── EduTek.API.Tests/        # xUnit integration + unit tests
├── EduTekPoc.sln            # Solution file
├── .gitignore
└── README.md
```

---

## Technology Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 10 / ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| Database | SQL Server (production), In-Memory (tests) |
| Auth (API) | JWT Bearer + Refresh Token |
| Auth (MVC) | `SessionAuthorizeAttribute` + `ISession` |
| Caching | `IMemoryCache` (Subject list, 5-min TTL) |
| Testing | xUnit, `WebApplicationFactory`, In-Memory EF |
| Frontend | Razor Views, Bootstrap 5 |

---

## Architecture Overview

```
 ┌──────────┐   HTTP    ┌──────────────────┐   DI    ┌──────────────────────┐
 │ EduTek   │──────────►│  EduTek.API       │────────►│  EduTek.Application  │
 │ .Web MVC │           │  Controllers       │         │  Services / DTOs     │
 └──────────┘           │  JWT Middleware    │         └──────────┬───────────┘
                        │  Exception Handler │                    │
                        └──────────────────-┘                    ▼
                                                     ┌──────────────────────┐
                                                     │  EduTek.Infrastructure│
                                                     │  EF Core / SQL Server │
                                                     └──────────────────────┘
```

### Role Hierarchy

| Role | Permissions |
|---|---|
| **Admin** | Full CRUD on all entities; user approval |
| **Teacher** | Create/edit Attendance, Marks, Feedback, Exams; read all academic data |
| **Student** | Read-only: Attendance, Marks, Exams, Feedback |

---

## JWT & Refresh Token Flow

1. `POST /api/Auth/login` → returns `{ token, refreshToken }`.
2. Client attaches `Authorization: Bearer <token>` on every API call.
3. On **401**: client calls `POST /api/Auth/refresh-token` with `{ refreshToken }`.
4. Server validates, rotates the refresh token (old one is revoked), returns a new pair.
5. `POST /api/Auth/logout` → revokes the current refresh token in the database.

Token settings (in `appsettings.json`):

```json
"Jwt": {
  "Key": "<your-256-bit-secret>",
  "Issuer": "EduTekAPI",
  "Audience": "EduTekClient",
  "ExpiryMinutes": 60
}
```

> `ClockSkew = TimeSpan.Zero` — tokens expire exactly at the configured time.

---

## Subject Caching

`SubjectService` uses `IMemoryCache` with a 5-minute sliding expiration:

- **Cache MISS** → queries the database, stores result under key `"subjects_all"`.
- **Cache HIT** → returns cached list, logs a hit message.
- **Eviction** → any Create/Update/Delete on a Subject removes `"subjects_all"` from the cache.

---

## API Endpoints

### Auth
| Method | Path | Roles | Description |
|---|---|---|---|
| POST | `/api/Auth/register` | Public | Register (pending approval) |
| POST | `/api/Auth/login` | Public | Login → JWT + refresh token |
| POST | `/api/Auth/refresh-token` | Public | Rotate refresh token |
| POST | `/api/Auth/logout` | Authenticated | Revoke refresh token |

### Admin
| Method | Path | Roles | Description |
|---|---|---|---|
| GET | `/api/Admin/pending-registrations` | Admin | List unapproved users |
| POST | `/api/Admin/approve` | Admin | Approve or reject a user |

### Department
| Method | Path | Roles |
|---|---|---|
| GET | `/api/Department` | Admin, Teacher, Student |
| GET | `/api/Department/{id}` | Admin, Teacher, Student |
| POST | `/api/Department` | Admin |
| PUT | `/api/Department/{id}` | Admin |
| DELETE | `/api/Department/{id}` | Admin |

### Class
| Method | Path | Roles |
|---|---|---|
| GET/GET{id} | `/api/Class` | Admin, Teacher, Student |
| POST/PUT/DELETE | `/api/Class` | Admin |

### Subject
| Method | Path | Roles |
|---|---|---|
| GET/GET{id} | `/api/Subject` | Admin, Teacher, Student |
| POST/PUT/DELETE | `/api/Subject` | Admin |

### Teacher
| Method | Path | Roles |
|---|---|---|
| GET/GET{id} | `/api/Teacher` | Admin, Teacher |
| POST/PUT/DELETE | `/api/Teacher` | Admin |

### Student
| Method | Path | Roles |
|---|---|---|
| GET/GET{id} | `/api/Student` | Admin, Teacher, Student |
| POST/PUT/DELETE | `/api/Student` | Admin |

### ClassSubject / TeacherSubjectClass
| Method | Path | Roles |
|---|---|---|
| GET | All | Admin, Teacher, Student |
| POST / DELETE | All | Admin |

### Attendance
| Method | Path | Roles |
|---|---|---|
| GET/GET{id} | `/api/Attendance` | Admin, Teacher, Student |
| POST/PUT | `/api/Attendance` | Admin, Teacher |
| DELETE | `/api/Attendance/{id}` | Admin |

### Exam
| Method | Path | Roles |
|---|---|---|
| GET/GET{id} | `/api/Exam` | Admin, Teacher, Student |
| POST/PUT | `/api/Exam` | Admin, Teacher |
| DELETE | `/api/Exam/{id}` | Admin |

### Mark
| Method | Path | Roles |
|---|---|---|
| GET/GET{id} | `/api/Mark` | Admin, Teacher, Student |
| POST/PUT | `/api/Mark` | Admin, Teacher |
| DELETE | `/api/Mark/{id}` | Admin |

### Feedback
| Method | Path | Roles |
|---|---|---|
| GET/GET{id} | `/api/Feedback` | Admin, Teacher, Student |
| POST/PUT | `/api/Feedback` | Admin, Teacher |
| DELETE | `/api/Feedback/{id}` | Admin |

---

## MVC Frontend Modules

| Module | Accessible To |
|---|---|
| Dashboard | All authenticated users |
| Department | All (Create/Edit/Delete → Admin) |
| Class | All (Create/Edit/Delete → Admin) |
| Subject | All (Create/Edit/Delete → Admin) |
| Teacher | Admin, Teacher |
| Student | Admin, Teacher, Student (self) |
| ClassSubject | All (Assign/Delete → Admin) |
| TeacherSubjectClass | All (Assign/Delete → Admin) |
| Attendance | All (Mark/Edit → Admin + Teacher; Delete → Admin) |
| Exam | All (Create/Edit → Admin + Teacher; Delete → Admin) |
| Mark | All (Create/Edit → Admin + Teacher; Delete → Admin) |
| Feedback | All (Create/Edit → Admin + Teacher; Delete → Admin) |
| Admin Approval | Admin only |

---

## Setup & Running

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or SQL Server Express / LocalDB)

### 1. Configure Connection String

Edit `EduTek.API/appsettings.json` and `EduTek.Web/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=EduTekDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2. Apply Migrations

```powershell
dotnet ef database update --project EduTek.Infrastructure --startup-project EduTek.API
```

### 3. Build the Solution

```powershell
dotnet build EduTekPoc.sln
```

### 4. Run Both Projects

In separate terminals:

```powershell
# Terminal 1 — Web API (https://localhost:7001)
dotnet run --project EduTek.API

# Terminal 2 — MVC Frontend (https://localhost:7002)
dotnet run --project EduTek.Web
```

The MVC app calls the API; ensure `ApiBaseUrl` in `EduTek.Web/appsettings.json` points to your API URL.

---

## Running Tests

```powershell
dotnet test EduTekPoc.sln
```

All tests use an **in-memory database** — no SQL Server required.

### Test Accounts (in-memory only)

| Username | Password | Role |
|---|---|---|
| `admin` | `Admin@123` | Admin |
| `teacher` | `Teacher@123` | Teacher |
| `student` | `Student@123` | Student |

### Test Coverage

| Suite | Tests |
|---|---|
| `RoleAuthorizationTests` | 401 without token, 401 invalid token, Admin 200, Teacher 403 on Admin APIs, Student 403 on Admin/Teacher mutations, Student 200 on read endpoints |
| `EndToEndWorkflowTests` | Admin full setup journey, Teacher+Student lifecycle (attendance→marks→feedback→exams), Refresh token rotation + logout revocation |
| `SessionAuthorizeFilterTests` | MVC unauthenticated redirect, wrong-role 403, correct-role passthrough |

---

## Security Notes

- JWT tokens validated against issuer, audience, lifetime, and signing key.
- `ClockSkew = TimeSpan.Zero` enforces strict expiry.
- Refresh tokens are single-use and rotated on each refresh.
- Logout revokes the refresh token in the database immediately.
- Session cookies are HTTP-only, essential, and HTTPS-only in production.
- Public registration cannot create Admin accounts.
- Exception middleware returns generic messages to clients (no stack traces).
