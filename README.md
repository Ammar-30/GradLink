# 🎓 GradLink - Alumni Networking Platform

A comprehensive ASP.NET Core MVC web application designed to connect university alumni, facilitate job postings, community events, and provide career resources.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Features](#features)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Default Credentials](#default-credentials)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

GradLink is a full-stack web application built with ASP.NET Core MVC that serves as a networking platform for university alumni. It enables users to:

- Create and share posts
- Apply for job opportunities
- Participate in community events
- Ask and answer questions in Q&A forums
- Access career advice and resources
- Connect with other alumni members

## 🏗️ Architecture

This project follows a **3-Layer Architecture** pattern:

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│         (GradLink - MVC)                │
│  - Controllers                          │
│  - Views (Razor Pages)                  │
│  - Static Files (CSS, JS, Images)       │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│         Business Logic Layer            │
│         (GradLink.Model)                │
│  - ViewModels                           │
│  - Data Transfer Objects                │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│         Data Access Layer               │
│         (GradLink.Repository)           │
│  - DbContext (Entity Framework)         │
│  - Entity Models                        │
│  - Database Operations                  │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│         Database                        │
│         (SQLite)                         │
└─────────────────────────────────────────┘
```

### Architecture Layers Explained

#### 1. **Presentation Layer (GradLink)**
- **Purpose**: Handles HTTP requests, user interface, and user interactions
- **Components**:
  - **Controllers**: Handle HTTP requests and coordinate between views and models
    - `AccountController`: Authentication, registration, profile management
    - `PostController`: Social media posts, comments, likes
    - `JobController`: Job listings and applications
    - `EventsController`: Community events management
    - `DashBoardController`: Dashboard features, Q&A, career advice, database viewer
    - `HomeController`: Landing page, about, contact
  - **Views**: Razor pages for rendering HTML
  - **wwwroot**: Static files (CSS, JavaScript, images)

#### 2. **Business Logic Layer (GradLink.Model)**
- **Purpose**: Contains ViewModels and data structures for data transfer
- **Components**:
  - `ViewModel/Account/`: Login, Register, UserProfile ViewModels
  - `ViewModel/Job/`: JobApplication ViewModel
  - `ViewModel/QuestionAnswer/`: Question and Answer ViewModels

#### 3. **Data Access Layer (GradLink.Repository)**
- **Purpose**: Manages database operations and data persistence
- **Components**:
  - `MSSQL/ORM/Context/`: `GradLinkDbContext` - Entity Framework DbContext
  - `MSSQL/ORM/Entities/`: Entity models representing database tables
    - User, Post, Job, Event, Question, Answer, Comment, Like, etc.

## 🛠️ Technology Stack

### Backend
- **.NET 8.0** - Modern C# framework
- **ASP.NET Core MVC** - Web framework
- **Entity Framework Core 8.0** - ORM for database operations
- **SQLite** - Lightweight, file-based database
- **BCrypt.Net** - Password hashing

### Frontend
- **Razor Pages** - Server-side rendering
- **Bootstrap 5** - CSS framework
- **JavaScript** - Client-side interactivity
- **Font Awesome** - Icons
- **jQuery** - DOM manipulation (if used)

### Authentication & Authorization
- **Cookie-based Authentication** - Custom authentication scheme
- **Claims-based Authorization** - Role-based access control (Admin/User)

## 📁 Project Structure

```
GradLink_App/
│
├── GradLink/                          # Main Web Application (Presentation Layer)
│   ├── Controllers/                   # MVC Controllers
│   │   ├── AccountController.cs      # Authentication & User Management
│   │   ├── DashBoardController.cs    # Dashboard & Admin Features
│   │   ├── EventsController.cs       # Events Management
│   │   ├── HomeController.cs        # Landing Pages
│   │   ├── JobController.cs         # Job Postings
│   │   └── PostController.cs        # Social Posts
│   │
│   ├── Views/                        # Razor Views
│   │   ├── Account/                 # Login, Register, Profile
│   │   ├── DashBoard/               # Dashboard Views
│   │   ├── Events/                  # Event Views
│   │   ├── Home/                    # Home, About, Contact
│   │   ├── Job/                     # Job Listings
│   │   ├── Post/                    # Social Feed
│   │   └── Shared/                  # Layouts (_Layout, _AdminLayout)
│   │
│   ├── wwwroot/                     # Static Files
│   │   ├── css/                     # Stylesheets
│   │   ├── js/                      # JavaScript Files
│   │   ├── img/                     # Images
│   │   ├── lib/                     # Third-party Libraries
│   │   └── Content/                 # User Uploads (CVs)
│   │
│   ├── Program.cs                   # Application Entry Point & Configuration
│   ├── appsettings.json             # Configuration File
│   └── GradLink.csproj              # Project File
│
├── GradLink.Model/                   # Business Logic Layer
│   └── ViewModel/                   # ViewModels for Data Transfer
│       ├── Account/
│       ├── Job/
│       └── QuestionAnswer/
│
├── GradLink.Repository/              # Data Access Layer
│   └── MSSQL/
│       └── ORM/
│           ├── Context/
│           │   └── GradLinkDbContext.cs  # EF Core DbContext
│           └── Entities/             # Entity Models (12 entities)
│               ├── User.cs
│               ├── Post.cs
│               ├── Job.cs
│               ├── Event.cs
│               └── ...
│
└── GradLink_App.sln                 # Solution File
```

## ✨ Features

### User Features
- ✅ User Registration & Authentication
- ✅ Profile Management
- ✅ Social Media Posts (Create, Like, Comment)
- ✅ Job Applications with CV Upload
- ✅ Event Participation
- ✅ Q&A Forum (Ask & Answer Questions)
- ✅ Career Advice Access
- ✅ Member Directory

### Admin Features
- ✅ Database Viewer (Prisma Studio-like interface)
- ✅ Job Posting Management
- ✅ User Management
- ✅ Content Moderation
- ✅ Analytics Dashboard

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A code editor (Visual Studio, VS Code, or Rider)
- Git (for cloning)

### Installation

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd GradLink_App
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   cd GradLink
   dotnet run
   ```

5. **Access the application**
   - Open your browser and navigate to: `http://localhost:5025`
   - Or `https://localhost:7056` for HTTPS

## 💾 Database Setup

The application uses **SQLite** database which is automatically created on first run.

### Automatic Setup
- Database file: `GradLink/GradLink.db`
- Created automatically when the application starts
- Initial data (roles, admin user) is seeded automatically

### Manual Database Inspection
You can inspect the database using SQLite command-line tools:

```bash
cd GradLink
sqlite3 GradLink.db

# View all tables
.tables

# View Users
SELECT * FROM Users;

# Exit
.quit
```

## 🔐 Default Credentials

**Admin Account** (created automatically):
- **Email**: `admin@gradlink.com`
- **Password**: `Admin123!`

⚠️ **Important**: Change the default admin password after first login!

## 📊 Database Schema

The application uses the following main entities:

- **Users** - User accounts and profiles
- **Posts** - Social media posts
- **Jobs** - Job listings
- **Events** - Community events
- **Questions** - Q&A questions
- **Answers** - Q&A answers
- **Comments** - Post comments
- **Likes** - Post likes
- **JobApplications** - Job applications
- **CareerAdvices** - Career advice posts
- **Roles** - User roles (Admin, User)
- **UserRoles** - User-Role mapping

## 🔧 Configuration

### Connection String
Edit `GradLink/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=GradLink.db"
  }
}
```

### Port Configuration
Edit `GradLink/Properties/launchSettings.json` to change ports.

## 🧪 Development

### Running in Development Mode
```bash
cd GradLink
dotnet run
```

### Building for Production
```bash
dotnet build -c Release
```

### Database Migrations
Currently using `EnsureCreated()`. For production, consider using migrations:

```bash
dotnet ef migrations add InitialCreate --project GradLink --startup-project GradLink
dotnet ef database update --project GradLink --startup-project GradLink
```

## 📝 API Endpoints

### Authentication
- `GET /Account/Login` - Login page
- `POST /Account/Login` - Login submission
- `GET /Account/Register` - Registration page
- `POST /Account/Register` - Registration submission
- `GET /Account/Logout` - Logout
- `GET /Account/Profile` - User profile
- `POST /Account/Profile` - Update profile

### Posts
- `GET /Post` - View all posts (requires authentication)
- `POST /Post/CreatePost` - Create new post
- `POST /Post/AddComment` - Add comment to post
- `POST /Post/ToggleLike` - Like/Unlike post
- `POST /Post/DeletePost` - Delete post

### Jobs
- `GET /Job` - View job listings
- `POST /Job/ApplyJob` - Apply for a job
- `POST /Job/Create` - Create job (Admin only)

### Events
- `GET /Events` - View all events
- `POST /Events/Create` - Create event

### Dashboard
- `GET /Dashboard/QNA` - Q&A forum
- `GET /Dashboard/Career` - Career advice
- `GET /Dashboard/DatabaseViewer` - Database viewer (Admin only)

## 🐛 Troubleshooting

### Port Already in Use
```bash
# Kill process on port 5025
kill -9 $(lsof -ti:5025)
```

### Database Issues
Delete the database file and restart:
```bash
rm GradLink/GradLink.db
dotnet run
```

### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

## 📄 License

This project is created for educational purposes as part of CE301 Final Year Project.

## 👤 Author

**Ammar S**
- University of Essex
- Final Year Project - CE301

## 🙏 Acknowledgments

- University of Essex
- .NET Community
- Bootstrap Team

---

**Note**: This is an academic project. For production use, consider:
- Implementing proper migrations
- Adding comprehensive error handling
- Setting up CI/CD pipelines
- Adding unit tests
- Implementing API rate limiting
- Adding logging and monitoring


