# Complaint Tracking System

A comprehensive ASP.NET Core 10 MVC-based complaint/ticket tracking system with role-based access control, real-time analytics, and ticket management capabilities.

## ?? Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
- [Running the Application](#running-the-application)
- [Default Login Credentials](#default-login-credentials)
- [Usage Guide](#usage-guide)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)
- [File Management](#file-management)
- [Configuration](#configuration)
- [Troubleshooting](#troubleshooting)
- [Future Enhancements](#future-enhancements)

## ? Features

### Core Functionality
- **Ticket Management** - Create, read, update, and delete support tickets
- **Status Workflow** - Opened ? Assigned ? In Progress ? Resolved ? Closed
- **Priority Levels** - Low, Medium, High priority classification
- **Category Classification** - Organize tickets by category
- **File Attachments** - Upload and download ticket attachments
- **Comments System** - Add comments and replies to tickets

### Authentication & Authorization
- **ASP.NET Identity Integration** - Secure user authentication
- **Role-Based Access Control (RBAC)** - Three roles: Admin, Agent, User
- **Fine-Grained Permissions** - Different capabilities per role
- **Secure Session Management** - Automatic logout and session handling

### Admin Dashboard
- **Real-Time Statistics** - Total, Open, and Resolved ticket counts
- **Resolution Rate Calculation** - Percentage of resolved tickets
- **Visual Analytics** - Chart.js powered visualizations
- **Category Breakdown** - Top categories and ticket distribution
- **Doughnut & Bar Charts** - Interactive data visualizations

### Ticket Assignment
- **Agent Assignment** - Admin can assign tickets to support agents
- **Workload Distribution** - Manage agent ticket allocation
- **Email Notifications** - Automatic notifications on assignment

### Pagination & Sorting
- **Page-Based Navigation** - Configurable page sizes
- **Multi-Column Sorting** - Sort by Date, Priority, or Status
- **Performance Optimized** - Database indexes on key columns

### Email Notifications
- Ticket creation notifications
- Ticket assignment alerts
- Status change updates
- (Currently mocked - ready for SMTP integration)

## ?? Prerequisites

- **.NET 10 SDK** - [Download](https://dotnet.microsoft.com/download)
- **SQL Server** - LocalDB, Express, or Standard Edition
- **Git** - For cloning the repository
- **Visual Studio 2022** or **VS Code** with C# extension
- **Package Manager Console** - Available in Visual Studio

## ?? Installation & Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/yourusername/ComplainTracking.git
cd ComplainTracking
```

### Step 2: Configure Database Connection

Edit `ComplainTracking/appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ComplainTrackingDb;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

**Connection String Options:**

- **LocalDB (Recommended for Development)**:
  ```
  Server=(localdb)\mssqllocaldb;Database=ComplainTrackingDb;Trusted_Connection=true;
  ```

- **SQL Server Express**:
  ```
  Server=.\SQLEXPRESS;Database=ComplainTrackingDb;Trusted_Connection=true;
  ```

- **Remote SQL Server**:
  ```
  Server=your-server.com;Database=ComplainTrackingDb;User Id=sa;Password=YourPassword;
  ```

### Step 3: Create Database & Run Migrations

Open **Package Manager Console** in Visual Studio:

```powershell
# Navigate to the project directory
cd ComplainTracking

# Create database and apply migrations
Add-Migration InitialCreate
Update-Database
```

This will:
- Create the database with all tables
- Create indexes on key columns
- Seed default roles and test users

### Step 4: Verify Installation

Build the solution to ensure everything compiles:

```bash
dotnet build
```

Expected output:
```
Build succeeded with 0 Warning(s)
```

## ?? Running the Application

### Option 1: Visual Studio

1. Open `ComplainTracking.sln` in Visual Studio 2022
2. Set `ComplainTracking` as the startup project
3. Press **F5** or click "Start" button
4. Application opens at `https://localhost:7001` (or port shown in console)

### Option 2: Command Line

```bash
cd ComplainTracking
dotnet run
```

Application will be available at: `https://localhost:5001` or `http://localhost:5000`

### Option 3: Using the Application

Once running:
1. Navigate to `https://localhost:7001` in your browser
2. Click **Login** in the top navigation
3. Enter credentials (see [Default Login Credentials](#default-login-credentials))
4. Click **Login**

## ?? Default Login Credentials

The application comes with pre-seeded test accounts:

| Role | Email | Password | Capabilities |
|------|-------|----------|--------------|
| **Admin** | `admin@complain.com` | `Admin@123` | View all tickets, assign, update status, delete, access dashboard |
| **Agent 1** | `agent1@complain.com` | `Agent@123` | View assigned tickets, view details |
| **Agent 2** | `agent2@complain.com` | `Agent@123` | View assigned tickets, view details |
| **User** | `user@complain.com` | `User@123` | Create tickets, view own tickets |

### ?? Security Note

Change these credentials in production! Update `SeedData.cs` before deploying.

## ?? Usage Guide

### As a Regular User

1. **Login** with `user@complain.com` / `User@123`
2. **Create Ticket**:
   - Click "Tickets" ? "Create New Ticket"
   - Fill in Title, Description, Category, and Priority
   - Optionally upload an attachment (max 10MB)
   - Click "Submit Ticket"
3. **View Tickets**: Go to "Tickets" to see all your submitted tickets
4. **Track Status**: Monitor ticket status as it moves through the workflow
5. **Download Attachments**: Click download icon on tickets with files

### As an Admin

1. **Login** with `admin@complain.com` / `Admin@123`
2. **View Dashboard**:
   - Click "Dashboard" to view analytics
   - See total tickets, open tickets, resolved count, and resolution rate
   - View category breakdown and ticket distribution charts
3. **Manage Tickets**:
   - Go to "Tickets" to view all tickets
   - Click on a ticket to view details and comments
4. **Update Status**:
   - Click "Update Status" dropdown
   - Change status: Opened ? Assigned ? In Progress ? Resolved ? Closed
5. **Assign Tickets**:
   - Click "Assign" on a ticket
   - Select an agent from the dropdown
   - Click "Assign" to notify the agent
6. **Delete Tickets**:
   - Click "Delete" button (use with caution)
   - Attachments are automatically cleaned up

### As a Support Agent

1. **Login** with `agent1@complain.com` / `Agent@123`
2. **View Assigned Tickets**:
   - Go to "Tickets" to see tickets assigned to you
   - Filter by your assignments (if available)
3. **Provide Support**:
   - Click on a ticket to view full details
   - Read ticket description and attachments
   - (Admin handles status updates)
4. **Track Progress**:
   - Monitor ticket status changes made by admin

## ??? Architecture

### Technology Stack

- **Framework**: ASP.NET Core 10 MVC
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Identity
- **Frontend**: HTML/CSS/Bootstrap with Chart.js
- **ORM**: Entity Framework Core with LINQ
- **Deployment**: Ready for Docker/Azure

### Design Patterns

- **Service Layer Pattern** - Business logic separation
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Loose coupling
- **DTO Pattern** - Data transfer objects for views
- **SOLID Principles** - Clean, maintainable code

### Security Features

- Password hashing with ASP.NET Identity
- CSRF token protection on forms
- SQL injection prevention via parameterized queries
- Secure file naming with GUIDs
- Role-based authorization filters
- HTTPS enforcement recommended

## ?? Project Structure

```
ComplainTracking/
??? Controllers/
?   ??? HomeController.cs          # Home & privacy pages
?   ??? TicketsController.cs       # Ticket CRUD & dashboard
?
??? Core/
?   ??? Interfaces/
?   ?   ??? ITicketService.cs      # Ticket business logic contract
?   ?   ??? ITicketCommentService.cs # Comment operations contract
?   ?   ??? IEmailService.cs       # Email notification contract
?   ?
?   ??? Services/
?       ??? TicketService.cs       # Ticket operations & logic
?       ??? TicketCommentService.cs # Comment management
?       ??? MockEmailService.cs    # Email notifications (mocked)
?
??? Data/
?   ??? ApplicationDbContext.cs    # EF Core database context
?   ??? Seeders/
?       ??? SeedData.cs            # Database seeding (roles & users)
?
??? Models/
?   ??? Entities/
?   ?   ??? ApplicationUser.cs     # Extended Identity user
?   ?   ??? Ticket.cs             # Main ticket entity
?   ?   ??? TicketComment.cs      # Comment entity
?   ?   ??? Enums/
?   ?       ??? TicketStatus.cs   # Status enum (5 states)
?   ?       ??? TicketPriority.cs # Priority enum (3 levels)
?   ?
?   ??? DTOs/
?       ??? TicketCreateDto.cs    # Ticket creation DTO
?       ??? TicketViewDto.cs      # Ticket display DTO
?       ??? PaginatedResult.cs    # Pagination wrapper
?       ??? DashboardStatsDto.cs  # Dashboard statistics DTO
?
??? Views/
?   ??? Tickets/                   # Ticket-related views
?   ?   ??? Index.cshtml          # Ticket list
?   ?   ??? Details.cshtml        # Ticket details
?   ?   ??? Create.cshtml         # Create ticket form
?   ?   ??? Edit.cshtml           # Edit ticket form
?   ?   ??? Dashboard.cshtml      # Admin dashboard
?   ?
?   ??? Home/                      # Home & privacy
?   ?   ??? Index.cshtml
?   ?   ??? Privacy.cshtml
?   ?
?   ??? Shared/
?       ??? _Layout.cshtml        # Master layout
?       ??? _LoginPartial.cshtml  # Auth UI
?       ??? Error.cshtml          # Error page
?       ??? _ValidationScriptsPartial.cshtml
?
??? wwwroot/
?   ??? css/                       # Stylesheets
?   ??? js/                        # JavaScript files
?   ??? lib/                       # Third-party libraries
?   ??? uploads/                   # File attachments folder
?
??? Program.cs                     # Dependency injection & middleware
??? appsettings.json               # Configuration
??? ComplainTracking.csproj        # Project file
```

## ?? API Endpoints

While primarily an MVC application, the `TicketService` provides underlying API functionality:

### Ticket Operations

| Operation | Endpoint | Method | Role Required |
|-----------|----------|--------|---------------|
| List Tickets | `/tickets` | GET | Authenticated |
| View Ticket | `/tickets/{id}` | GET | Authenticated |
| Create Ticket | `/tickets/create` | POST | User+ |
| Edit Ticket | `/tickets/edit/{id}` | POST | Admin |
| Delete Ticket | `/tickets/delete/{id}` | POST | Admin |
| Update Status | `/tickets/updatestatus/{id}` | POST | Admin |
| Assign Ticket | `/tickets/assign/{id}` | POST | Admin |
| Dashboard | `/tickets/dashboard` | GET | Admin |

### Query Parameters

```
/tickets?page=1&pageSize=10&sortBy=date&sortOrder=desc
```

Parameters:
- `page` - Page number (default: 1)
- `pageSize` - Items per page (default: 10)
- `sortBy` - Sort column (date, priority, status)
- `sortOrder` - asc or desc

## ?? Database Schema

### Tables Overview

**Tickets**
- ID, Title, Description, Category, Priority, Status
- SubmitterId, AssignedAgentId, CreatedAt, UpdatedAt
- Indexes: (Status), (CreatedAt), (SubmitterId), (AssignedAgentId)

**TicketComments**
- ID, TicketId, UserId, CommentText, CreatedAt
- Foreign Key: Ticket

**AspNetUsers** (Identity)
- Id, UserName, Email, PhoneNumber, Roles

**AspNetRoles** (Identity)
- Id, Name (Admin, Agent, User)

**AspNetUserRoles** (Identity Mapping)
- UserId, RoleId

## ?? File Management

### Attachment Handling

- **Storage Location**: `/wwwroot/uploads/`
- **Max File Size**: 10MB (configurable in `Program.cs`)
- **Naming**: Files renamed with GUID for security
- **Cleanup**: Automatic deletion when ticket is deleted
- **Security**: Files not directly accessible via URL

### Configuration

To change max file size, edit `Program.cs`:

```csharp
var options = new FormOptions
{
    MultipartBodyLengthLimit = 10_485_760 // 10MB
};
```

## ?? Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ComplainTrackingDb;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Key Settings

| Setting | Description | Default |
|---------|-------------|---------|
| `ConnectionStrings:DefaultConnection` | Database connection string | LocalDB |
| `Logging:LogLevel:Default` | Log level | Information |
| `AllowedHosts` | CORS allowed hosts | * (all) |

### Production Configuration

For production deployment, update:

1. **Connection String** - Use SQL Server in cloud (Azure SQL, AWS RDS)
2. **CORS/AllowedHosts** - Restrict to your domain
3. **Logging Level** - Set to Warning or higher
4. **HTTPS** - Enforce in Middleware
5. **Authentication** - Update seed credentials

## ?? Troubleshooting

### Database Connection Errors

**Error**: "Cannot connect to SQL Server"

**Solutions**:
1. Verify SQL Server service is running
2. Check connection string in `appsettings.json`
3. Ensure LocalDB is installed: `sqllocaldb info`
4. Reset LocalDB: `sqllocaldb delete mssqllocaldb` then recreate

**Commands**:
```powershell
# List available LocalDB instances
sqllocaldb info

# Create new instance
sqllocaldb create mssqllocaldb

# Start instance
sqllocaldb start mssqllocaldb
```

### Migration Issues

**Error**: "The model backing the context has changed since the database was created"

**Solutions**:
1. Delete database and rerun migrations:
   ```powershell
   Update-Database 0
   Update-Database
   ```
2. Or create a new migration:
   ```powershell
   Add-Migration Fix
   Update-Database
   ```

### File Upload Issues

**Error**: "Cannot upload file"

**Solutions**:
1. Create `/wwwroot/uploads/` folder if missing
2. Check folder write permissions
3. Verify file size is under 10MB
4. Check disk space availability

### Authentication Issues

**Error**: "Invalid login credentials"

**Solutions**:
1. Verify user exists: Check `AspNetUsers` in database
2. Reset database and reseed: `Update-Database 0` then `Update-Database`
3. Clear browser cookies and try again
4. Check user role assignments in `AspNetUserRoles`

### Port Already in Use

**Error**: "Address already in use"

**Solutions**:
```bash
# Find process using port 7001
netstat -ano | findstr :7001

# Kill process
taskkill /PID <ProcessID> /F

# Or use different port
dotnet run --urls https://localhost:7002
```

### HTTPS Certificate Issues

**Error**: "NET::ERR_CERT_AUTHORITY_INVALID"

**Solutions**:
1. Create self-signed certificate:
   ```bash
   dotnet dev-certs https --clean
   dotnet dev-certs https --trust
   ```
2. Use HTTP for local development:
   ```bash
   dotnet run --urls http://localhost:5000
   ```

## ?? Common Tasks

### Change Database

Update `appsettings.json` connection string and rebuild:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=YourDb;User Id=sa;Password=YourPassword;"
}
```

### Add New Role

1. Edit `SeedData.cs` - Add role to seeding
2. Create migration:
   ```powershell
   Add-Migration AddNewRole
   Update-Database
   ```
3. Assign users to role in controller

### Enable Real Email Notifications

1. Create new implementation of `IEmailService`
2. Configure SMTP in `appsettings.json`:
   ```json
   "EmailSettings": {
     "SmtpServer": "smtp.gmail.com",
     "SmtpPort": 587,
     "SenderEmail": "your-email@gmail.com",
     "SenderPassword": "your-app-password"
   }
   ```
3. Update `Program.cs` dependency injection
4. Replace `MockEmailService` with your implementation

### Modify Ticket Status Workflow

1. Edit `TicketStatus.cs` enum - Add/remove statuses
2. Update `Details.cshtml` - Update status dropdown
3. Update `Dashboard.cshtml` - Update statistics logic
4. Add migration if entity changed

### Change Pagination Size

In `TicketsController.cs`, modify:

```csharp
const int pageSize = 10;  // Change this value
```

## ?? Performance Optimization

### Database Indexes

The application includes indexes on:
- `Tickets.Status`
- `Tickets.CreatedAt`
- `Tickets.SubmitterId`
- `Tickets.AssignedAgentId`

For large datasets, consider additional indexes:
```sql
CREATE INDEX IX_Ticket_Category ON Tickets(Category);
CREATE INDEX IX_Ticket_Priority ON Tickets(Priority);
```

### Caching

For dashboard statistics, implement caching:

```csharp
// In TicketService
var cacheKey = "dashboard_stats";
if (!_cache.TryGetValue(cacheKey, out DashboardStatsDto stats))
{
    stats = await ComputeStats();
    _cache.Set(cacheKey, stats, TimeSpan.FromMinutes(5));
}
```

### Query Optimization

- Use `.AsNoTracking()` for read-only queries
- Implement lazy loading for related entities
- Use `.Select()` for projection instead of mapping DTOs manually

## ?? Deployment

### Azure App Service

1. Create App Service in Azure
2. Create SQL Database
3. Publish from Visual Studio:
   - Right-click project ? Publish
   - Choose Azure App Service
4. Configure connection string in Azure portal
5. Run migrations in Azure

### Docker

Create `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ComplainTracking.csproj", ""]
RUN dotnet restore "ComplainTracking.csproj"
COPY . .
RUN dotnet build "ComplainTracking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ComplainTracking.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ComplainTracking.dll"]
```

Build and run:
```bash
docker build -t complaint-tracker .
docker run -p 80:80 complaint-tracker
```

## ?? Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [ASP.NET Identity](https://docs.microsoft.com/aspnet/identity/overview)
- [Bootstrap Documentation](https://getbootstrap.com/docs)
- [Chart.js Documentation](https://www.chartjs.org/docs/)

## ?? Contributing

1. Create a feature branch (`git checkout -b feature/AmazingFeature`)
2. Commit your changes (`git commit -m 'Add AmazingFeature'`)
3. Push to the branch (`git push origin feature/AmazingFeature`)
4. Open a Pull Request

## ?? License

This project is licensed under the MIT License - see the LICENSE file for details.

## ?? Support

For issues and questions:
1. Check the [Troubleshooting](#troubleshooting) section
2. Review the [Quick Start Guide](QUICK_START.md)
3. Check [Implementation Summary](IMPLEMENTATION_SUMMARY.md)
4. Create an issue in the repository

## ?? Roadmap

- [ ] Unit tests (xUnit)
- [ ] Integration tests
- [ ] Real email notifications
- [ ] Advanced search and filtering
- [ ] Ticket export (PDF/Excel)
- [ ] Bulk operations
- [ ] REST API with Swagger
- [ ] Real-time notifications with SignalR
- [ ] Mobile app

---

**Last Updated**: 2024  
**Version**: 1.0  
**Status**: ? Production Ready
