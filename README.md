# Url Shortener

A Full Stack URL shortening service built with **C#**, **ASP.NET Core**, **Angular**, and **Entity Framework Core**. This application allows users to shorten long URLs into shorter, more manageable links.

## Features

- **URL Shortening**: Convert long URLs into short, easy-to-remember links.
- **Redirection**: Automatically redirect users to the original URL when they visit a short link.
- **Persistence**: Data stored in a database using Entity Framework Core.
- **User Interface**: A modern front-end built with Angular.

## Technologies Used

- **Backend**: ASP.NET Core with Entity Framework Core for database management.
- **Frontend**: Angular (v19) with modern libraries for state management and UI components.
- **Database**: Microsoft SQL Server.
- **Build Tools**: Node.js for frontend dependencies and .NET CLI for backend operations.

- ## Getting Started

### Prerequisites

Ensure the following are installed on your system:

- .NET SDK (6.0 or later)
- Node.js (16.x or later)
- SQL Server

  ### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/semenyaka08/url-shortener.git
   cd url-shortener
   ```

2. **Configure the database**:
   - Update the connection string in `UrlShortener.Api/appsettings.Development.json` to match your SQL Server setup.
   - Run database migrations:
     ```bash
     cd UrlShortener.Api
     dotnet ef database update
     ```
3. **Run the backend**:
   ```bash
   cd UrlShortener.Api
   dotnet run
   ```
4. **Run the frontend**:
   ```bash
   cd client
   npm install
   ng serve
   ```
5. **Access the application**:
   Open your browser and navigate to `http://localhost:4200`.

## Project Structure

- `UrlShortener.Api`: ASP.NET Core backend, handling APIs logic.
- `UrlShortener.Core`: Core handling business logic.
- `UrlShortener.Core.Tests`: Includes unit tests for the core business logic.
- `UrlShortener.Dal`: Data access layer to interact with Db.
- `client`: Angular frontend.

---
