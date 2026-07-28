# BookSmart

An online bookstore built with ASP.NET Web Forms (.NET Framework 4.7.2), C# and
SQL Server, with a Bootstrap front end. Written for the BSc (Hons) Software
Engineering programme at the University of Technology, Mauritius.

## Features

**Accounts** — registration, forms-authentication login, logout and password
recovery. Anonymous users can browse; everything else requires a session.

**Shopping** — browse the catalogue, add to cart, place an order, pay, and
review order history and confirmations.

**Admin** — dashboard, management screens and reports.

## Structure

```
BookSmart.sln
BookSmart/
  Account/      Login, Registration, ForgotPassword, Logout
  User/         BrowseBooks, Cart, Order, Payment, OrderHistory, OrderConfirmation
  Admin/        Admin, Dashboard, Reports
  Class/        Business and data-access classes
  MasterPages/  Shared layout
  images/       Book covers and assets
  Web.config    Connection string and authorisation rules
Database Backup/
  BookSmart_20250807.bak   SQL Server backup — restore this to get the schema and seed data
```

## Running it locally

Needs Visual Studio (2019 or later) with the ASP.NET workload, and SQL Server
with SQL Server Management Studio.

1. **Restore the database.** In SSMS, restore
   `Database Backup/BookSmart_20250807.bak` as a database named `BookSmart`.

2. **Set the connection string.** Open `BookSmart/Web.config` and replace the
   placeholders with your own server and login:

   ```xml
   <add name="BookSmartDB"
        connectionString="Server=YOUR_SERVER\YOUR_INSTANCE;Database=BookSmart;User Id=YOUR_DB_USER;Password=YOUR_DB_PASSWORD;"
        providerName="System.Data.SqlClient"/>
   ```

   Or use Windows authentication and drop the user and password entirely:

   ```xml
   connectionString="Server=YOUR_SERVER\YOUR_INSTANCE;Database=BookSmart;Integrated Security=True;"
   ```

3. **Open `BookSmart.sln`**, let NuGet restore the packages, and run.

> **Don't commit real credentials.** `Web.config` is tracked, so anything you
> put in it is public. Keep the placeholders in the committed copy, or move the
> connection string into a `Web.local.config` (already gitignored) and reference
> it with `configSource`.

## Hosting

GitHub Pages can't host this — it serves static files only, and Web Forms needs
IIS and a live SQL Server to render a page. It needs Windows hosting, such as
Azure App Service with Azure SQL, or a free ASP.NET host like MonsterASP.NET.
