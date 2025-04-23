# Test Dashboard

A web application for creating, managing, and taking tests online.

## Prerequisites

- Node.js LTS version (18.x or later)
- .NET SDK 7.0 or later
- SQL Server (LocalDB or higher)

## Project Structure

- `TestDashboard.API/` - Backend API (.NET)
- `TestDashboard.Web/` - Frontend application (React + TypeScript)
- `TestDashboard.Data/` - Data access layer and migrations
- `TestDashboard.Domain/` - Domain models and business logic

## Getting Started

### Backend Setup

1. Install .NET SDK from https://dotnet.microsoft.com/download
2. Navigate to the API project:
   ```
   cd TestDashboard.API
   ```
3. Install Entity Framework tools:
   ```
   dotnet tool install --global dotnet-ef
   ```
4. Run database migrations:
   ```
   dotnet ef database update
   ```
5. Start the API:
   ```
   dotnet run
   ```

### Frontend Setup

1. Navigate to the Web project:
   ```
   cd TestDashboard.Web
   ```
2. Install dependencies:
   ```
   npm install
   ```
3. Start the development server:
   ```
   npm start
   ```

The application will be available at:
- Frontend: http://localhost:3000
- Backend API: http://localhost:5000

## Features

- User authentication and authorization
- Test creation and management
- Test taking with timer
- Results tracking and analysis
- Responsive design using Material-UI

## Technology Stack

### Frontend
- React
- TypeScript
- Redux for state management
- Material-UI for components
- Axios for API communication

### Backend
- ASP.NET Core
- Entity Framework Core
- JWT Authentication
- SQL Server 