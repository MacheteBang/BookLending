# 📚 MacheteBang.BookLending

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Status](https://img.shields.io/badge/status-in%20development-yellow.svg)

> _Because every book deserves to be read, not just sit on a shelf!_

A modern, clean, and extensible Book Lending API built with .NET 9, minimal APIs, and vertical slice architecture.

## 🔍 Overview

MacheteBang.BookLending is a RESTful API that helps libraries manage their book inventory and lending processes. It offers a streamlined way to catalog books, track their availability, and manage the borrowing process.

## ✨ Features

- **📚 Book Management** - Add, retrieve, and search for books in the library catalog
- **📖 Book Copy Management** - Track individual copies of books with different conditions
- **🔄 Vertical Slice Architecture** - Feature-focused organization for simpler maintenance and better isolation
- **📝 OpenAPI Documentation** - Interactive API documentation with Scalar
- **🧪 API Testing** - Bruno collections for easy API testing
- **🔍 Health Checks** - Endpoint monitoring for system health
- **📊 Telemetry** - Built-in telemetry for monitoring and debugging
- **🌐 Minimal API** - Modern .NET minimal API approach for lean endpoints

## 🛠️ Technology Stack

- **Framework**: .NET 9
- **API Style**: ASP.NET Core Minimal APIs
- **Database**: SQL Server with Entity Framework Core
- **Documentation**: OpenAPI with Scalar UI
- **Testing**: Bruno API Client
- **Logging**: Serilog
- **Error Handling**: ErrorOr pattern

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- Visual Studio 2022, VS Code, or JetBrains Rider

### Installation & Setup

1. Clone the repository

   ```
   git clone https://github.com/yourusername/MacheteBang.BookLending.git
   cd MacheteBang.BookLending
   ```

2. Configure your database connection string

   - Create or modify `src/MacheteBang.BookLending.Api/appsettings.Local.json` with: ```json
     {
     "ConnectionStrings": {
     "BooksDb": "Server=localhost;Database=BookLending;Trusted_Connection=True;TrustServerCertificate=True;"
     }
     }

   ```

   - Or add the connection string to another appropriate `appsettings.*.json` file

   ```

3. Build the solution

   ```
   dotnet build
   ```

4. Run the API

   ```
   cd src/MacheteBang.BookLending.Api
   dotnet run
   ```

5. Access the API documentation
   ```
   https://localhost:7128/scalar
   ```

## 🧪 API Testing

The project includes Bruno API collections for testing:

1. Install Bruno: https://www.usebruno.com/
2. Open the Bruno collections in the `/tools/bruno` directory
3. Use the provided environments and request templates to test the API

## 📖 API Usage Examples

See the [API Usage Documentation](docs/API-Usage.md) for detailed examples of how to use each endpoint.

## 🔮 Roadmap

- User authentication and authorization
- Book borrowing and return functionality
- Due date tracking and notifications
- Advanced search capabilities
- Ratings and reviews

## 🤝 Contributing

Contributions are welcome! Feel free to:

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- Built with ❤️ using .NET 9
- Inspired by vertical slice architecture
- Made with lots of ☕ and 🎵
