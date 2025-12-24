# ⚡ Quick Start Guide

## 🎯 Project Overview

**GradLink** is an Alumni Networking Platform built with ASP.NET Core MVC following a 3-Layer Architecture.

## 🏗️ Architecture in 3 Minutes

```
┌─────────────┐
│   Browser   │ ← User interacts here
└─────────────┘
       ↓
┌─────────────┐
│  GradLink   │ ← Controllers handle requests, Views render HTML
│  (MVC App)  │
└─────────────┘
       ↓
┌─────────────┐
│GradLink.Model│ ← ViewModels (data structures)
└─────────────┘
       ↓
┌─────────────┐
│GradLink.Repo│ ← Database operations (Entity Framework)
└─────────────┘
       ↓
┌─────────────┐
│   SQLite    │ ← Database (file: GradLink.db)
└─────────────┘
```

## 📦 What Each Layer Does

### 1. GradLink (Presentation)
- **Controllers**: Handle HTTP requests (like `/Post`, `/Account/Login`)
- **Views**: HTML pages (Razor syntax)
- **wwwroot**: CSS, JavaScript, images

### 2. GradLink.Model (Business Logic)
- **ViewModels**: Data structures for forms and data transfer
- Example: `LoginViewModel` has Email and Password properties

### 3. GradLink.Repository (Data Access)
- **DbContext**: Manages database connection
- **Entities**: Database table models (User, Post, Job, etc.)

## 🚀 Running the Project

```bash
cd GradLink
dotnet run
```

Visit: `http://localhost:5025`

## 🔐 Login

- **Email**: `admin@gradlink.com`
- **Password**: `Admin123!`

## 📁 Key Files

- `Program.cs` - Application startup and configuration
- `appsettings.json` - Database connection string
- `Controllers/` - Request handlers
- `Views/` - HTML templates
- `GradLink.db` - SQLite database (auto-created)

## 🗄️ Database

- **Type**: SQLite (file-based)
- **Location**: `GradLink/GradLink.db`
- **Tables**: 12 tables (Users, Posts, Jobs, Events, etc.)
- **Auto-created**: Yes, on first run

## 🎨 Main Features

1. **User Management**: Register, login, profile
2. **Social Posts**: Create, like, comment
3. **Job Board**: Post and apply for jobs
4. **Events**: Create and view events
5. **Q&A Forum**: Ask and answer questions
6. **Database Viewer**: Admin tool (like Prisma Studio)

## 🔧 Common Commands

```bash
# Build
dotnet build

# Run
dotnet run

# Clean
dotnet clean

# Restore packages
dotnet restore
```

## 🐛 Troubleshooting

**Port in use?**
```bash
kill -9 $(lsof -ti:5025)
```

**Database issues?**
```bash
rm GradLink/GradLink.db
dotnet run  # Recreates database
```

## 📚 More Info

- See `README.md` for full documentation
- See `ARCHITECTURE.md` for detailed architecture
- See `GITHUB_SETUP.md` for GitHub upload guide


