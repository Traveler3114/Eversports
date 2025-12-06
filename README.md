# Eversports

A cross-platform mobile application built with .NET MAUI for connecting sports enthusiasts and organizing sports activities together.

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Supported Platforms](#supported-platforms)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Usage](#usage)
- [Available Sports](#available-sports)
- [Localization](#localization)
- [API Integration](#api-integration)
- [Development](#development)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

Eversports is a mobile application designed to help sports enthusiasts find partners for various sports activities. The app allows users to create profiles, post availability for sports activities, search for others looking to play, and communicate through an integrated chat system.

## ✨ Features

### User Management
- **User Registration & Authentication**: Secure user registration and login with JWT token-based authentication
- **User Profiles**: Personal profiles with user information management
- **Role-based Access**: Support for different user roles including admin functionality

### Finding Sports Partners
- **Create Availability Posts**: Post your availability for sports activities with detailed information
  - Select multiple sports you want to play
  - Specify location (country, city, detailed location)
  - Set available date and time ranges
  - Add descriptions and additional details
- **Search for Activities**: Browse and filter sports activities posted by other users
- **Location-based Search**: Find activities in specific countries and cities

### Communication
- **Real-time Chat**: Built-in messaging system to communicate with other users
- **Activity-specific Chat Rooms**: Dedicated chat for each sports activity posting

### Administrative Features
- **Admin Panel**: Special administrative interface for managing the platform
- **User Management**: Administrative controls for user oversight

### Localization
- **Multi-language Support**: Available in multiple languages (English, Croatian)
- **Dynamic Language Switching**: Change language on-the-fly without restarting the app

## 📱 Supported Platforms

Eversports is built with .NET MAUI and supports the following platforms:

- **Android** (API level 21+)
- **iOS** (15.0+)
- **macOS Catalyst** (15.0+)
- **Windows** (Windows 10, version 10.0.17763.0+)

## 🛠 Technologies Used

### Framework & Platform
- **.NET 9.0** - Latest .NET framework
- **Microsoft MAUI** - Cross-platform framework for building native mobile and desktop apps
- **XAML** - UI markup language for defining application interfaces

### Libraries & Dependencies
- **CsvHelper** (v33.0.1) - CSV reading and writing
- **System.ServiceModel** (v6.0.*) - WCF service model libraries
  - Duplex communication
  - Federation
  - HTTP bindings
  - Named pipes
  - TCP bindings
  - Security features

### Architecture Patterns
- **MVVM (Model-View-ViewModel)** - Separation of UI and business logic
- **Services Pattern** - Encapsulated business logic and API communication
- **Shell Navigation** - MAUI Shell for app navigation structure

## 📋 Prerequisites

To build and run Eversports, you need:

- **Visual Studio 2022** (v17.13 or later) with the following workloads:
  - .NET Multi-platform App UI development
  - For Android development: Android SDK
  - For iOS development: Xcode (Mac required)
- **.NET 9.0 SDK** or later
- **Platform-specific SDKs**:
  - Android: Android SDK API 21 or higher
  - iOS: Xcode 14+ (macOS required)
  - Windows: Windows 10 SDK (10.0.17763.0)

## 🚀 Installation

### Clone the Repository

```bash
git clone https://github.com/Traveler3114/Eversports.git
cd Eversports
```

### Restore Dependencies

```bash
dotnet restore Eversports.sln
```

### Build the Solution

```bash
dotnet build Eversports.sln
```

### Run the Application

For Android:
```bash
dotnet build -t:Run -f net9.0-android
```

For iOS (on macOS):
```bash
dotnet build -t:Run -f net9.0-ios
```

For Windows:
```bash
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

## ⚙️ Configuration

### API Endpoint

The application connects to a backend API hosted at:
```
https://traveler3114.ddns.net/EversportsAPI/
```

API endpoints are configured in the service classes located in the `Services` directory:
- `UserService.cs` - User authentication and management
- `LookingToPlayService.cs` - Sports activity posts management
- `ChatService.cs` - Messaging functionality

### SSL Certificate Validation

**Note**: The application currently disables SSL certificate validation for development purposes. This is configured in each service's HttpClient handler:

```csharp
var handler = new HttpClientHandler()
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
};
```

**⚠️ Security Warning**: This should be properly configured with valid certificates in production.

### Storage

The app uses `SecureStorage` for storing sensitive data like JWT authentication tokens:
- JWT tokens are stored securely using platform-specific secure storage mechanisms
- Tokens persist between app sessions

## 📁 Project Structure

```
Eversports/
├── Eversports.sln                 # Visual Studio solution file
└── Eversports/                    # Main application project
    ├── App.xaml                   # Application definition
    ├── App.xaml.cs                # Application code-behind
    ├── MauiProgram.cs            # App startup and configuration
    ├── Localization.cs           # Localization utilities
    │
    ├── Models/                    # Data models
    │   ├── UserInfo.cs           # User model
    │   ├── LookingToPlay.cs      # Sports activity post model
    │   ├── AvailableDateTime.cs  # Date/time availability model
    │   ├── CountryCities.cs      # Location model
    │   └── Response.cs           # API response model
    │
    ├── Services/                  # Business logic and API services
    │   ├── UserService.cs        # User management service
    │   ├── LookingToPlayService.cs # Activity management service
    │   └── ChatService.cs        # Messaging service
    │
    ├── Pages/                     # XAML pages (screens)
    │   ├── LoginPage.xaml        # Login screen
    │   ├── RegistrationPage.xaml # User registration
    │   ├── ProfilePage.xaml      # User profile
    │   ├── LookingToPlayPage.xaml # Create activity post
    │   ├── FindToPlayPage.xaml   # Browse activities
    │   ├── ChatPage.xaml         # Chat/messaging
    │   └── AdminPage.xaml        # Admin dashboard
    │
    ├── Views/                     # Reusable UI components
    │   ├── FindToPlayView.xaml   # Activity list item view
    │   ├── ItemView.xaml         # Generic item view
    │   ├── MessageView.xaml      # Chat message view
    │   └── UserView.xaml         # User item view
    │
    ├── Shells/                    # Shell navigation containers
    │   ├── AppShellLogin.xaml    # Login navigation shell
    │   └── AppShellMain.xaml     # Main app navigation shell
    │
    ├── Resources/                 # Application resources
    │   ├── Fonts/                # Custom fonts
    │   ├── Images/               # Image assets
    │   ├── AppIcon/              # App icon
    │   ├── Splash/               # Splash screen
    │   ├── Raw/                  # Raw assets
    │   ├── Strings.resx          # Default localization strings
    │   └── Strings.hr.resx       # Croatian localization
    │
    └── Platforms/                 # Platform-specific code
        ├── Android/              # Android-specific implementations
        ├── iOS/                  # iOS-specific implementations
        ├── MacCatalyst/          # macOS-specific implementations
        └── Windows/              # Windows-specific implementations
```

## 💻 Usage

### First Time Setup

1. **Launch the Application**: Start the app on your device or simulator
2. **Create an Account**: 
   - Tap "Register" on the login screen
   - Fill in your name, surname, email, and password
   - Submit the registration form
3. **Login**: Use your credentials to log into the app

### Creating a Sports Activity Post

1. Navigate to the "Looking to Play" page
2. Select the sport(s) you want to play from the picker
3. Choose your location (country and city)
4. Add a detailed location (e.g., specific park or facility)
5. Set your available date and time ranges
6. Add a description
7. Submit the post

### Finding Sports Partners

1. Go to the "Find to Play" page
2. Browse available sports activities
3. Filter by:
   - Sport type
   - Location
   - Date/time availability
4. View details of interesting activities
5. Join the chat to connect with the organizer

### Chatting with Other Users

1. Select an activity you're interested in
2. Open the chat interface
3. Send messages to the activity organizer
4. Coordinate details for meeting up

### Profile Management

1. Access your profile from the navigation menu
2. View your information
3. Update profile details as needed
4. Logout when done

## 🏃 Available Sports

The application supports a wide variety of sports:

- Running
- Hiking
- Tennis
- Table Tennis
- Cycling
- Football
- Basketball
- Cricket
- Baseball
- Golf
- Volleyball
- Swimming
- Boxing
- Martial Arts
- Ice Hockey
- Badminton
- Skiing
- Snowboarding
- Skateboarding
- Surfing
- Rock Climbing
- Yoga
- Gym
- Wrestling
- Rowing
- Rugby
- American Football
- Ice Skating
- Roller Skating
- Squash
- Handball

*Users can select multiple sports when creating an activity post.*

## 🌍 Localization

Eversports supports multiple languages with dynamic language switching:

### Supported Languages
- **English** (en) - Default
- **Croatian** (hr) - Hrvatski

### Changing Language

The app uses .NET resource files (`.resx`) for localization:
- `Resources/Strings.resx` - Default English strings
- `Resources/Strings.hr.resx` - Croatian translations

Language can be changed at runtime using the `Localization.SetLanguage()` method:

```csharp
await Localization.SetLanguage("hr"); // Switch to Croatian
await Localization.SetLanguage("default"); // Switch to default (English)
```

### Adding New Languages

1. Create a new resource file: `Resources/Strings.[language-code].resx`
2. Add translations for all string keys from the default `Strings.resx`
3. The language will be automatically available in the app

## 🔌 API Integration

The application communicates with a RESTful backend API. The API base URL is:
```
https://traveler3114.ddns.net/EversportsAPI/
```

### API Endpoints

#### User Management
- `POST /UserFunctions/Register.php` - Register new user
- `POST /UserFunctions/Login.php` - User authentication
- `POST /UserFunctions/GetUser.php` - Get user information

#### Activity Management
- `POST /LookingToPlayFunctions/AddLookingToPlay.php` - Create activity post
- `POST /LookingToPlayFunctions/DeleteLookingToPlay.php` - Delete activity post
- `POST /LookingToPlayFunctions/GetLookingToPlay.php` - Get activity listings

#### Messaging
- `POST /Messaging/SendMessage.php` - Send chat message
- `GET /Messaging/GetMessages.php` - Retrieve chat messages

### Authentication

The API uses JWT (JSON Web Token) authentication:
1. User logs in with credentials
2. API returns a JWT token
3. Token is stored securely using `SecureStorage`
4. Token is included in subsequent API requests
5. Token format: Sent in request body as `jwt` parameter or query string

## 👨‍💻 Development

### Debug Mode

Debug mode includes special testing features:
```csharp
#if DEBUG
    // Admin credentials for testing
    if (EmailEntry.Text == "admin" && PasswordEntry.Text == "admin")
    {
        // Skip normal authentication
    }
#endif
```

### Building for Release

To build a release version:

```bash
dotnet build Eversports.sln -c Release
```

### Running Tests

Currently, the project doesn't include a dedicated test suite. Consider adding:
- Unit tests for business logic in Services
- UI tests for critical user flows
- Integration tests for API communication

### Code Style

The project follows standard C# coding conventions:
- PascalCase for public members
- camelCase for private fields (with underscore prefix `_privateField`)
- Async methods suffixed with `Async`
- Proper null handling with nullable reference types enabled

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. **Fork the Repository**
   ```bash
   git clone https://github.com/Traveler3114/Eversports.git
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Your Changes**
   - Follow existing code style and conventions
   - Test your changes on multiple platforms if possible
   - Update documentation as needed

4. **Commit Your Changes**
   ```bash
   git commit -m "Add: description of your changes"
   ```

5. **Push to Your Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Create a Pull Request**
   - Provide a clear description of your changes
   - Reference any related issues
   - Ensure all platforms build successfully

### Development Guidelines

- Maintain cross-platform compatibility
- Use async/await for all I/O operations
- Handle exceptions gracefully with user-friendly error messages
- Follow MVVM pattern for UI logic separation
- Add XML documentation comments for public APIs
- Test on multiple platforms before submitting PR

## 📄 License

This project's license is not specified. Please contact the repository owner for licensing information.

## 📞 Support

For support, questions, or feature requests:
- Open an issue on GitHub: [Eversports Issues](https://github.com/Traveler3114/Eversports/issues)
- Contact the repository owner: [Traveler3114](https://github.com/Traveler3114)

## 🔐 Security Considerations

**Important Security Notes:**

1. **SSL Certificate Validation**: Currently disabled for development. Enable proper certificate validation in production.

2. **Secure Storage**: JWT tokens are stored using platform-specific secure storage, but ensure your backend implements proper token expiration and refresh mechanisms.

3. **API Communication**: All API calls should use HTTPS in production environments.

4. **Input Validation**: Always validate user input on both client and server sides.

## 🗺️ Roadmap

Potential future enhancements:

- [ ] Push notifications for new messages and activity matches
- [ ] User ratings and reviews system
- [ ] Advanced filtering and search capabilities
- [ ] Social media integration
- [ ] Activity history and statistics
- [ ] In-app calendar integration
- [ ] Photo sharing for activities
- [ ] Group activities and team formation
- [ ] Subscription/payment integration for premium features
- [ ] Offline mode support
- [ ] Unit and integration testing suite

## 🙏 Acknowledgments

- Built with [.NET MAUI](https://dotnet.microsoft.com/apps/maui)
- Uses [CsvHelper](https://joshclose.github.io/CsvHelper/) for CSV operations
- UI built with XAML

---

**Happy Sports Finding! 🏃‍♂️🎾⚽🏀**
