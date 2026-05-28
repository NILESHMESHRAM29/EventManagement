🎉 Event Management API

A robust **ASP.NET Core 8 Web API** for managing events, students, QR-based check-in/scan workflows, ID card generation, and role-based access control.

---

🚀 Features

- **Event Management** — Create, update, and soft-delete events
- **Student Management** — Register students with unique IDs, QR codes, and ID cards
- **QR Code Generation** — Auto-generate QR codes for each student
- **ID Card PDF Generation** — Generate printable ID cards using QuestPDF
- **JWT Authentication** — Secure token-based authentication
- **Role & Permission System** — Fine-grained access control with roles and permissions
- **Scan / Check-in System** — Track student attendance via QR scans
- **Section & Volunteer Management** — Organize students into sections and assign volunteers
- **Import Batch Support** — Bulk import of student data
- **Swagger UI** — Interactive API documentation out of the box
- **Docker Support** — Fully containerized for easy deployment

---
🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 |
| Language | C# (.NET 8) |
| Database | PostgreSQL (via Npgsql EF Core) |
| ORM | Entity Framework Core 8 |
| Auth | JWT Bearer Tokens |
| PDF Generation | QuestPDF |
| QR Code | QRCoder |
| Mapping | AutoMapper 12 |
| API Docs | Swashbuckle / Swagger |
| Containerization | Docker |

---

📁 Project Structure

```
EventManagement/
├── Controllers/          # API endpoint controllers
│   ├── AuthController.cs
│   ├── EventsController.cs
│   ├── StudentsController.cs
│   ├── ScansController.cs
│   ├── IdCardController.cs
│   ├── SectionsController.cs
│   ├── SessionsController.cs
│   ├── RolesController.cs
│   ├── PermissionsController.cs
│   ├── PermissionRolesController.cs
│   ├── SectionVolunteersController.cs
│   ├── ImportBatchesController.cs
│   └── UsersController.cs
├── Models/               # Entity / domain models
├── DTOs/                 # Data Transfer Objects
├── Data/                 # EF Core DbContext & value converters
├── Mappings/             # AutoMapper profiles
├── Migrations/           # EF Core database migrations
├── Service/              # Business logic services
│   ├── JwtService.cs
│   ├── QRCodeGenerator.cs
│   ├── IdCardPdfService.cs
│   └── PasswordHasherService.cs
├── Program.cs            # App entry point & DI setup
└── Dockerfile
```

---

⚙️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/) *(optional)*

---

🔧 Getting Started

1. Clone the repository

```bash
git clone https://github.com/NILESHMESHRAM29/EventManagement.git
cd EventManagement
```

2. Configure the database

Update `appsettings.json` (or use environment variables) with your PostgreSQL connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=yourpassword"
  },
  "Jwt": {
    "Key": "your_super_secret_key_here",
    "Issuer": "EventManagementAPI",
    "Audience": "EventManagementClient"
  }
}
```

3. Apply database migrations

```bash
cd EventManagement
dotnet ef database update
```

4. Run the application

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).  
Swagger UI: `https://localhost:5001/swagger`

---

🐳 Running with Docker

```bash
docker build -t event-management-api .
docker run -p 8080:80 \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;..." \
  -e Jwt__Key="your_secret_key" \
  event-management-api
```

---

🔐 Authentication

The API uses **JWT Bearer authentication**.

1. Register or login via `/api/auth`
2. Copy the token from the response
3. In Swagger UI, click **Authorize** and enter: `Bearer <your_token>`
4. All protected endpoints will now accept your requests

---

📋 API Endpoints Overview

| Module | Base Route |
|---|---|
| Authentication | `/api/auth` |
| Events | `/api/events` |
| Students | `/api/students` |
| Sections | `/api/sections` |
| Scans | `/api/scans` |
| Sessions | `/api/sessions` |
| ID Cards | `/api/idcard` |
| Import Batches | `/api/importbatches` |
| Roles | `/api/roles` |
| Permissions | `/api/permissions` |
| Permission Roles | `/api/permissionroles` |
| Section Volunteers | `/api/sectionvolunteers` |
| Users | `/api/users` |

Full interactive documentation is available via Swagger UI when the app is running.

---

🌐 CORS

CORS is configured for:
- `http://localhost:4200` (local Angular dev)
- `https://eventmanagement-lwxj.onrender.com` (production frontend)

To allow other origins, update the `AngularPolicy` in `Program.cs`.

---

🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a Pull Request

---
📄 License

This project is open-source. Feel free to use and modify it.

---
