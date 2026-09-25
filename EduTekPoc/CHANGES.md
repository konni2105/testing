# EduTek Pending Work Changes

This document describes the work completed to close the remaining roadmap items: role-based testing, end-to-end testing, UI refinement, code cleanup and security review, and final solution setup.

## 1. Role-based testing

Added `EduTek.API.Tests` with integration tests that call the Web API through `WebApplicationFactory`.

`RoleAuthorizationTests` verifies:

- **401** when no JWT is sent to a protected endpoint
- **Admin** can call Admin-only APIs (`/api/Admin/pending-registrations`) and shared GET endpoints
- **Teacher** receives **403** on Admin-only APIs and department create
- **Student** can GET academic data (**200**) and receives **403** on teacher/admin mutations (student create, attendance create)

MVC pages now enforce the same rules with `SessionAuthorizeAttribute`:

- Unauthenticated users are redirected to login
- Authenticated users with the wrong role receive **403 Access Denied**
- Admin-only create/edit/delete stays on Admin
- Attendance, marks, exams, and feedback create/edit allow Admin and Teacher

## 2. End-to-end testing

`EndToEndWorkflowTests` covers the main journeys from the roadmap:

- **Admin:** login → register pending user → approve → department → class → teacher → student → subject → class subject → teacher assignment → exam
- **Teacher:** login → assigned classes → attendance → marks → feedback
- **Student:** login → attendance/marks/feedback reads used by the dashboard
- **Refresh token:** rotate on refresh, revoke on logout, reject reused token (**401**)

Tests use an in-memory database and seeded Admin/Teacher/Student users.

## 3. UI refinement

- Role-based Bootstrap navigation (Admin dropdown, Teacher assignments, Student academic links)
- Login, register, home, dashboard, tables, forms, alerts, and validation styling
- Dashboard cards use consistent shadows and spacing
- Exam create/edit is available to teachers as well as admins, matching API permissions
- Global form/table CSS so remaining CRUD screens pick up Bootstrap-like controls

## 4. Code cleanup and security

### JWT

- Issuer, audience, and signing key are read from configuration (no hardcoded fallbacks)
- Token validation now checks issuer, audience, lifetime, and signing key
- `ClockSkew` remains zero so expiry is enforced immediately

### Logout and refresh-token revocation

- API `POST /api/Auth/logout` clears the refresh token in the database
- MVC logout calls that API, then clears session
- Login no longer stays on the login page after success; it redirects to the dashboard

### Exception handling

- Domain `throw new Exception(...)` calls were changed to `InvalidOperationException`
- API middleware maps:
  - `InvalidOperationException` / `ArgumentException` → **400**
  - `UnauthorizedAccessException` → **401**
  - `KeyNotFoundException` → **404**
  - all other exceptions → **500** with a generic message and `traceId`

### Other cleanup

- Fixed Student GET role list (`"Admin,Teacher, Student"` → `"Admin,Teacher,Student"`)
- Public registration cannot create Admin accounts (Teacher = 2, Student = 3)
- Student profile creation after approval uses role **name**, not a hardcoded role id
- Pending user DTO now includes `RoleId`
- Session cookies are HTTP-only, essential, and HTTPS-only
- Unused Swashbuckle references removed from Application/Infrastructure
- Dead JWT generation code removed from the API auth controller
- Subject cache logging uses `ILogger` instead of `Console.WriteLine`

## 5. Final build

A solution file was added: `EduTekPoc.sln`.

Expected verification:

```text
dotnet clean EduTekPoc.sln
dotnet build EduTekPoc.sln
dotnet test EduTekPoc.sln
```

Run both the API and MVC sites, then walk through login for Admin, Teacher, and Student.

## Test users used by automated tests

| Username | Password     | Role    |
|----------|--------------|---------|
| admin    | Admin@123    | Admin   |
| teacher  | Teacher@123  | Teacher |
| student  | Student@123  | Student |

These users exist only in the in-memory test database, not in SQL Server.

## Files added

- `EduTek.API.Tests/` (factory, role tests, E2E tests)
- `EduTek.Web/Filters/SessionAuthorizeAttribute.cs`
- `EduTek.Web/Views/Shared/AccessDenied.cshtml`
- `EduTekPoc.sln`
- `CHANGES.md` (this file)

---

## 6. Final verification & git setup (session 2)

### Build & test

```
dotnet clean EduTekPoc.sln   → 0 warnings, 0 errors
dotnet build EduTekPoc.sln   → 0 warnings, 0 errors
dotnet test  EduTekPoc.sln   → 17/17 tests passed
```

Test suites executed:
- `RoleAuthorizationTests` (9 tests)
- `EndToEndWorkflowTests` (3 tests)
- `SessionAuthorizeFilterTests` (3 tests)
- Additional role tests added in session 1 (2 more)

### Git repository initialised

`git init` → initial commit containing all source files; `.gitignore` excludes
`bin/`, `obj/`, `.vs/`, `.idea/`, test-result artefacts, local DB files,
OS noise, and log files.

### Documentation

- Added `README.md` with architecture diagram, API endpoint table, role matrix,
  JWT/refresh-token flow, caching design, setup instructions, and test account list.
- Updated `CHANGES.md` (this entry).
