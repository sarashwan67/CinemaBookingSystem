# 🎬 Cinema Booking System

A web application for managing a cinema: movies, halls, show times, seats, customers, and bookings. Built with **ASP.NET Core MVC** as the final project of the *Web Development using .NET* course at the Information Technology Institute (ITI).

## Features

- Full CRUD for Movies, Cinema Halls, Show Times, Seats, Customers, and Bookings
- Movie poster upload
- Search, sort, and order on Movies, Halls, Seats, and Customers
- Duplicate-booking prevention: the same seat cannot be booked twice for the same show time
- Customer details page listing all of the customer's bookings

## Entities & Relationships

- Movie → Show Time (one-to-many)
- Cinema Hall → Show Time (one-to-many)
- Cinema Hall → Seat (one-to-many)
- Customer + Show Time + Seat → Booking
- Customer → Bookings (one-to-many)

## Tech Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (Code First + Migrations)
- SQL Server
- Bootstrap, HTML, CSS

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022+

### Setup

1. Clone the repository:
   ```
   git clone https://github.com/sarashwan67/CinemaBookingSystem.git
   ```
2. Open the project in Visual Studio.
3. Open `Data/AppDbContext.cs` and change the server name in the connection string to your own SQL Server instance:
   ```csharp
   Server=YOUR_SERVER_NAME;Database=Cinema Booking;Trusted_Connection=True;TrustServerCertificate=True
   ```
4. Create the database from the migrations. In Package Manager Console run:
   ```
   Update-Database
   ```
5. Run the project (F5) and open `https://localhost:7050`.

## Screenshots

A full walkthrough of every screen is available here:
[CinemaBookingSystem_Screenshots.pdf](docs/CinemaBookingSystem_Screenshots.pdf)

## Author

**Sara Mohamed Rashwan**
Faculty of Computers and Information, Helwan University
Course: Web Development using .NET (144 hrs), Information Technology Institute (ITI)