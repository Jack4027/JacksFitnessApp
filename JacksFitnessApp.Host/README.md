# JacksFitnessApp

A full stack fitness tracking web application built with .NET 10 and Angular 21, deployed on Microsoft Azure.

**Live Demo:** [https://calm-smoke-0800bf803-preview.westeurope.7.azurestaticapps.net](https://calm-smoke-0800bf803-preview.westeurope.7.azurestaticapps.net)

---

## Features

**Workout tracking**
- Log workout sessions with strength sets (reps, weight) and cardio sets (duration, distance, heart rate)
- Automatic personal record detection on every logged set
- Filter exercises by muscle group
- View full workout history with expandable session details

**Programme builder**
- Create structured training programmes with goals (strength, hypertrophy, endurance etc)
- Build weekly schedules with named training days
- Assign planned exercises with target sets and rep ranges

**Nutrition tracking**
- Daily nutrition log with macro totals (calories, protein, carbs, fat)
- Add meals by type (breakfast, lunch, dinner, pre/post workout etc)
- Search food items via Open Food Facts API with client-side cache — previously searched terms load instantly without a network request
- Macro calculations based on quantity in grams

**Body metrics**
- Log dated body measurements — weight, body fat, muscle mass, and tape measurements
- View full measurement history

**AI Personal Coach**
- Conversational AI coach powered by the Anthropic Claude API
- Real fitness data (workouts, nutrition, body metrics) injected into the system prompt for contextual coaching
- Persistent conversation history with inline title editing
- Markdown rendering for structured AI responses

**Dashboard**
- Overview of recent workouts, personal records, and latest body metrics

---

## Tech Stack

### Backend
- .NET 10 / C#
- Clean Architecture (Domain, Application, Infrastructure, Host layers)
- CQRS with MediatR
- Entity Framework Core with SQL Server
- ASP.NET Identity + JWT authentication
- FluentValidation
- AutoMapper 12

### Frontend
- Angular 21 (standalone components)
- Angular Material
- RxJS
- ngx-markdown

### Infrastructure
- Docker / Docker Compose
- Azure App Service (API)
- Azure Static Web Apps (Frontend)
- Azure SQL (Database)
- Docker Hub

---

## Architecture

The backend follows Clean Architecture with strict dependency rules:

```
Domain          ← no external dependencies
Application     ← depends on Domain only
Infrastructure  ← depends on Application and Domain
Host            ← depends on everything, wires it all together
```

**Layer responsibilities:**

```
JacksFitnessApp.Domain          — Entities, enums, repository interfaces
JacksFitnessApp.Application     — CQRS handlers, DTOs, validators, AutoMapper profiles
JacksFitnessApp.Infrastructure  — EF Core DbContext, repositories, Anthropic service, OpenFoodFacts service
JacksFitnessApp.Host            — Minimal API endpoints, JWT auth, middleware, Program.cs
```

**Request flow:**
```
Angular Component
  → Service (HTTP)
    → Auth Interceptor (attaches JWT)
      → ASP.NET Core (JWT validation, CORS)
        → Minimal API Endpoint
          → MediatR (routes to handler)
            → Handler (business logic)
              → Repository Interface
                → EF Core (SQL Server)
              → AutoMapper (entity → DTO)
            → Response
          → JSON serialisation
        → HTTP response
      → Angular HttpClient
    → Observable emission
  → Component updates UI
```

---

## Domain Model

**Training context**
- `Exercise` — master exercise library with support for global and user-created custom exercises
- `Programme` → `ProgrammeWeek` → `ProgrammeDay` → `PlannedExercise` — training plan hierarchy
- `WorkoutSession` → `WorkoutSet` / `CardioSet` — logged training data

**Nutrition context**
- `NutritionLog` → `Meal` → `MealItem` → `FoodItem` — daily food intake hierarchy
- Macro calculations are computed properties on `MealItem` based on quantity and per-100g values from `FoodItem`

**Metrics context**
- `BodyMetric` — dated body measurement snapshots with nullable fields for partial logging

**Coach context**
- `CoachConversation` → `CoachMessage` — persistent AI conversation history per user

---

## Key Design Decisions

**Personal record detection** — calculated at the point of logging rather than stored separately. Each new set is compared against the current best for that exercise and user. Simple, always accurate, no synchronisation required.

**Food search cache-aside** — Open Food Facts results are saved to the local database on first search. Subsequent searches check locally first, falling back to the external API only when needed. Client-side caching with an RxJS `Map` further reduces requests within a session — repeated searches return instantly from memory.

**Separate strength and cardio set entities** — `WorkoutSet` and `CardioSet` are separate entities rather than a single table with nullable columns. A strength set captures reps and weight; a cardio set captures duration, distance, and heart rate. Separate entities keep the domain model clean and avoid sparse tables.

**AI coach context injection** — the user's real fitness data (recent workouts, nutrition logs, body metrics, personal records) is fetched and injected into the Claude API system prompt on every conversation. This gives the coach genuine context rather than generic responses.

**Macro safety** — all nutrition values from Open Food Facts are sanitised and clamped to realistic ranges before saving. Calories are capped at 900 per 100g (pure fat), macronutrients at 100g per 100g.

---

## Getting Started

### Prerequisites

- Docker Desktop
- .NET 10 SDK
- Node.js 20+

### Environment Variables

Copy `.env.example` to `.env` in the solution root and fill in the values:

```bash
cp .env.example .env
```

```env
ANTHROPIC_API_KEY=your-anthropic-api-key
JWT_KEY=your-jwt-secret-minimum-32-characters
SA_PASSWORD=your-sql-server-password
```

An Anthropic API key is required for the AI Coach feature. Obtain one at [console.anthropic.com](https://console.anthropic.com).

### Run with Docker

```bash
docker-compose up --build
```

The app will be available at:
- Frontend: [http://localhost:4200](http://localhost:4200)
- API: [http://localhost:8080](http://localhost:8080)

On first run EF Core migrations execute automatically and the exercise library is seeded.

---

## Azure Deployment

### API — Azure App Service

The API is containerised and deployed to Azure App Service from Docker Hub. Environment variables are configured in the App Service portal.

```bash
docker build -t <dockerhub-username>/jacksfitnessapp-api:latest .
docker push <dockerhub-username>/jacksfitnessapp-api:latest
```

### Frontend — Azure Static Web Apps

```bash
cd fitnessapp-ui
ng build --configuration production
swa deploy ./dist/fitnessapp-ui/browser --deployment-token <your-token>
```

### Database — Azure SQL

Migrations are applied via the .NET CLI with the Azure connection string configured in `appsettings.Development.json` (gitignored).

```bash
dotnet ef database update --project JacksFitnessApp.Infrastructure --startup-project JacksFitnessApp.Host
```

---

## Project Structure

```
JacksFitnessApp/
├── JacksFitnessApp.Domain/
├── JacksFitnessApp.Application/
├── JacksFitnessApp.Infrastructure/
├── JacksFitnessApp.Host/
├── JacksFitnessApp.Tests/
├── fitnessapp-ui/              — Angular frontend
├── docker-compose.yml
├── Dockerfile
├── .env.example
└── README.md
```

---

## Known Limitations

- Personal record flags are not retroactively updated if a PR set is deleted
- JWT tokens expire after 60 minutes — the app redirects to login on 401
- MediatR is used under its development licence — a commercial licence is required for production use at scale

---

## Source Code

[https://github.com/Jack4027/JacksFitnessApp](https://github.com/Jack4027/JacksFitnessApp)
