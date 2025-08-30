# 🐕 WUPHF! - The Ultimate Social Networking Experience

Welcome to WUPHF!, the revolutionary social networking platform envisioned by Ryan Howard from The Office! This monorepo contains three .NET applications that bring Ryan's ambitious dream to life.

> *"Facebook, Twitter, SMS, Email, Chat, and even prints to the nearest printer!"* - Ryan Howard

## 🚀 Project Overview

This monorepo implements WUPHF using modern .NET 10 technology with three interconnected applications:

### 📱 WUPHF.Mobile (MAUI App)
A cross-platform mobile application built with .NET MAUI that allows users to send WUPHFs on the go.

**Features:**
- Send messages to multiple channels simultaneously
- View WUPHF history
- Cross-platform support (iOS, Android, Windows, macOS)
- Ryan Howard-themed UI with Office references

### 🌐 WUPHF.Web (Blazor App)
A modern web frontend built with Blazor Server that provides the full WUPHF experience in a browser.

**Features:**
- Responsive web interface
- Real-time message sending
- Message history with filtering
- About page with Ryan Howard's vision
- Bootstrap-powered UI with custom WUPHF styling

### ⚡ WUPHF.Api (Web API)
A robust backend API built with ASP.NET Core that handles all WUPHF operations and channel integrations.

**Features:**
- RESTful API endpoints
- Multi-channel message delivery simulation
- Message history management
- OpenAPI documentation
- Ryan Howard's motivational quotes API

### 📚 WUPHF.Shared (Class Library)
A shared library containing common models, DTOs, and constants used across all applications.

## 🏗️ Architecture

```
WUPHF Monorepo
├── src/
│   ├── WUPHF.Mobile/         # .NET MAUI Mobile App
│   ├── WUPHF.Web/            # Blazor Server Web App
│   ├── WUPHF.Api/            # ASP.NET Core Web API
│   └── WUPHF.Shared/         # Shared Library
└── WUPHF.sln                 # Solution File
```

## 🌟 WUPHF Channels

WUPHF supports the following communication channels:

- 📘 **Facebook** - Social media posts
- 🐦 **Twitter** - Tweets and mentions
- 💬 **SMS** - Text messages
- 📧 **Email** - Email notifications
- 💭 **Chat** - Instant messaging
- 🖨️ **Printer** - Physical printouts (Ryan's favorite!)
- 💼 **LinkedIn** - Professional networking
- 📸 **Instagram** - Visual posts

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK (Preview)
- Visual Studio 2025 or VS Code
- For mobile development: MAUI workload installed

### Installation

**Quick Start (PowerShell):**
```powershell
.\quickstart.ps1
```

**Manual Installation:**

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd monorepo
   ```

2. **Install MAUI workload (for mobile app):**
   ```bash
   dotnet workload install maui
   ```

3. **Restore packages:**
   ```bash
   dotnet restore
   ```

4. **Build the solution:**
   ```bash
   dotnet build
   ```

### Running the Applications

#### Web API (Backend)
```bash
cd src/WUPHF.Api/WUPHF.Api
dotnet run
```
The API will be available at `http://localhost:5043` with OpenAPI documentation at `/openapi/v1.json`.

#### Blazor Web App
```bash
cd src/WUPHF.Web/WUPHF.Web
dotnet run
```
The web app will be available at `https://localhost:5001`.

#### MAUI Mobile App
```bash
cd src/WUPHF.Mobile/WUPHF.Mobile
dotnet run --framework net9.0-windows10.0.19041.0  # For Windows
```

## 📱 Mobile App Screenshots

The MAUI app features:
- 🏠 **Home Page** - Welcome screen with WUPHF branding
- 📱 **Send WUPHF** - Multi-channel message composition
- 📜 **History** - Message history with status tracking

## 🌐 Web App Features

The Blazor web app includes:
- 🎨 **Modern UI** - Bootstrap-powered responsive design
- 🚀 **Send WUPHF Page** - Channel selection and message composition
- 📊 **History Page** - Detailed message tracking with search
- 💡 **About Ryan** - Learn about the visionary behind WUPHF

## 🔌 API Endpoints

The Web API provides the following endpoints:

- `POST /api/wuphf/send` - Send a WUPHF message
- `GET /api/wuphf/history` - Get message history
- `GET /api/wuphf/{id}` - Get specific message
- `GET /api/wuphf/quotes` - Get Ryan Howard quotes
- `GET /api/wuphf/health` - Health check

## 🎭 The Office Connection

This project is inspired by Ryan Howard's WUPHF venture from Season 7 of The Office. While Ryan's original idea didn't quite work out, we've brought his vision to life with modern technology:

> *"Imagine you're in an accident and you need to contact someone. Instead of being like 'Help me,' you'd be like 'WUPHF me!'"* - Ryan Howard

## 💰 Investment Opportunity

Just like Ryan Howard, we're always looking for investors! WUPHF represents the convergence of everything, everywhere, all at once!

**Key Features:**
- ✅ Multi-channel messaging
- ✅ Real-time delivery
- ✅ Mobile and web platforms
- ✅ Printer integration (revolutionary!)
- ✅ Ryan Howard approved

## 🤝 Contributing

We welcome contributions to make WUPHF even better! Please feel free to:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

Remember: Every contribution brings us closer to Ryan's dream of owning the biggest social networking site in the world!

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🎬 Credits

- Inspired by Ryan Howard's vision from The Office
- Built with love using .NET 10
- Special thanks to Dunder Mifflin for the inspiration

---

*"I thought I was going to be rich. I mean, I still might be."* - Ryan Howard

**WUPHF! The ultimate social networking experience!**
