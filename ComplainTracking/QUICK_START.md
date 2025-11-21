# Quick Start Guide - Complaint Tracking System

## Running the Application

### Prerequisites
- .NET 10 SDK installed
- SQL Server (LocalDB or Express)
- Visual Studio or VS Code

### Step 1: Update Database Connection
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ComplainTrackingDb;Trusted_Connection=true;"
}
```

### Step 2: Run Migrations
Open Package Manager Console and run:
```powershell
Add-Migration InitialCreate
Update-Database
```

### Step 3: Run the Application
```bash
dotnet run
```

The application will be available at: `https://localhost:7001` (or the port shown in console)

## Accessing the Application

### Navigation Links
- **Home** - Home page
- **Tickets** - View all tickets (requires authentication)
- **Dashboard** - Admin dashboard (Admin only)
- **Privacy** - Privacy policy

### User Workflows

#### As a Regular User:
1. Login with: `user@complain.com` / `User@123`
2. Click "Tickets" ? "New Ticket"
3. Fill in Title, Description, Category, Priority
4. Optionally upload an attachment
5. Click "Submit Ticket"
6. View your submitted tickets and their status

#### As an Admin:
1. Login with: `admin@complain.com` / `Admin@123`
2. Access "Dashboard" to view statistics
3. Click "Tickets" to manage all tickets
4. Click on a ticket to:
   - View all details and comments
   - Update status (Open ? Assigned ? In Progress ? Resolved ? Closed)
   - Assign to an agent
   - Delete if needed
5. View analytics and category breakdown

#### As an Agent:
1. Login with: `agent1@complain.com` / `Agent@123`
2. View tickets assigned to you
3. View ticket details to provide support
4. (Admin manages assignments)

## API Endpoints (JSON API)

### Get All Tickets
```
GET /api/tickets?page=1&sort=date_desc
```

### Response Format
```json
{
  "items": [
    {
      "id": 1,
      "title": "Network Issue",
      "description": "Cannot connect to VPN",
      "priority": "High",
      "status": "Opened",
      "createdAt": "2024-01-15T10:30:00Z",
      "category": "Network"
    }
  ],
  "totalCount": 10,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

## Ticket Lifecycle

```
???????????      ????????????      ??????????????
? Opened  ?  ?   ? Assigned ?  ?   ? In Progress?
???????????      ????????????      ??????????????
                                           ?
                                           ?
                                    ??????????????
                                    ? Resolved   ?
                                    ??????????????
                                           ?
                                           ?
                                    ??????????????
                                    ?  Closed    ?
                                    ??????????????
```

## File Attachments

- **Location**: `/uploads/` folder
- **Max Size**: 10MB (configurable)
- **Format**: Any file type (configure restrictions if needed)
- **Security**: Files are renamed with GUID to prevent direct access

## Dashboard Metrics

- **Total Tickets**: Count of all tickets
- **Open Tickets**: Tickets with status "Opened"
- **Resolved Tickets**: Tickets with status "Resolved"
- **Resolution Rate**: (Resolved / Total) × 100%
- **Category Breakdown**: Top 5 categories

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure LocalDB instance exists: `(localdb)\mssqllocaldb`

### File Upload Issues
- Check `/wwwroot/uploads` folder exists
- Verify write permissions on the folder
- Check file size limits in configuration

### Authentication Issues
- Ensure user is registered in database
- Check user role assignments
- Clear browser cache and try again

### Email Notifications
- Currently using MockEmailService (doesn't send real emails)
- To enable real emails, implement SMTP configuration
- Update `IEmailService` implementation in `Program.cs`

## Key Files to Customize

1. **Program.cs** - Dependency injection and middleware configuration
2. **appsettings.json** - Connection strings and app settings
3. **TicketService.cs** - Business logic for ticket operations
4. **MockEmailService.cs** - Email notification logic
5. **Dashboard.cshtml** - Admin dashboard UI

## Common Tasks

### Add File Type Validation
Edit `TicketService.CreateTicketAsync()` to validate file extensions

### Change Email Notifications
Update `IEmailService.SendEmailAsync()` calls in TicketService methods

### Add New Ticket Status
1. Add to `TicketStatus` enum
2. Update switch statements in views
3. Add handling in `UpdateStatusAsync()`

### Modify Pagination Size
Change `const int pageSize = 10;` in TicketsController.Index()

## Performance Tips

- Database has indexes on: Status, CreatedAt, SubmitterId, AssignedAgentId
- Use pagination for large ticket lists
- Consider caching dashboard statistics
- Archive old closed tickets to improve query performance

## Support & Documentation

- See `IMPLEMENTATION_SUMMARY.md` for detailed architecture
- Check code comments for specific implementations
- Review Entity relationships in `ApplicationDbContext.cs`
