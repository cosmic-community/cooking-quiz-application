# Quiz API - C# Backend

A robust ASP.NET Core Web API backend for the Cooking Quiz Application.

## Features

- RESTful API endpoints for quiz management
- User authentication with JWT tokens
- Quiz creation, management, and gameplay
- Leaderboard and achievements system
- Integration with Cosmic CMS for data synchronization
- SQL Server database with Entity Framework Core
- Swagger API documentation

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code with C# extensions

## Getting Started

### 1. Clone the repository
```bash
cd backend/QuizAPI
```

### 2. Install dependencies
```bash
dotnet restore
```

### 3. Update configuration
Edit `appsettings.json` with your configuration:
- Database connection string
- JWT secret key
- Cosmic CMS credentials

### 4. Run database migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the application
```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `POST /api/auth/refresh` - Refresh JWT token

### Quizzes
- `GET /api/quiz` - Get all quizzes
- `GET /api/quiz/{slug}` - Get quiz by slug
- `GET /api/quiz/featured` - Get featured quizzes
- `POST /api/quiz` - Create new quiz (Admin only)
- `PUT /api/quiz/{id}` - Update quiz (Admin only)
- `DELETE /api/quiz/{id}` - Delete quiz (Admin only)
- `POST /api/quiz/{slug}/start` - Start quiz session
- `POST /api/quiz/{slug}/submit-answer` - Submit answer

### Results
- `GET /api/result/user/{userId}` - Get user results
- `GET /api/result/leaderboard` - Get leaderboard
- `POST /api/result/submit` - Submit quiz result
- `GET /api/result/statistics/{userId}` - Get user statistics

## Database Schema

The application uses Entity Framework Core with the following models:
- Users - User accounts and profiles
- Quizzes - Quiz definitions
- Questions - Quiz questions
- QuestionOptions - Answer options for questions
- Categories - Quiz categories
- Results - Quiz completion results
- AnswerRecords - Individual answer records
- Achievements - Achievement definitions
- UserAchievements - User achievement associations

## Authentication

The API uses JWT bearer tokens for authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your-jwt-token}
```

## CORS Configuration

CORS is configured to allow requests from the Next.js frontend (localhost:3000 by default). Update the CORS policy in `Program.cs` for production domains.

## Deployment

### Azure App Service
1. Create an Azure App Service
2. Configure connection strings in Application Settings
3. Deploy using Visual Studio or Azure CLI

### Docker
```bash
docker build -t quiz-api .
docker run -p 5000:80 quiz-api
```

## Testing

Run unit tests:
```bash
dotnet test
```

## Contributing

1. Create a feature branch
2. Make your changes
3. Submit a pull request

## License

MIT