# JacksFitnessApp

A full-stack fitness tracking application for logging workouts, tracking nutrition, monitoring body metrics, and building training programmes. Built as a portfolio project demonstrating Clean Architecture, domain-driven design, and modern full-stack development practices.

---

## Tech stack

**Backend**
- .NET 10 Minimal API
- Clean Architecture (Domain, Application, Infrastructure, Host)
- Entity Framework Core — code first migrations
- ASP.NET Identity — user registration and authentication
- JWT Bearer authentication
- MediatR — CQRS query and command pipeline
- AutoMapper — entity to DTO mapping
- FluentValidation
- SQL Server LocalDB

**Frontend**
- Angular 21 (standalone components)
- Angular Material
- RxJS
- Chart.js / ng2-charts

**External API**
- Open Food Facts — food search and barcode lookup with local cache-aside pattern

---

## Features

**Workout tracking**
- Create workout sessions and log strength sets with reps, weight, and rest time
- Log cardio sets with duration, distance, heart rate, and calories
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
- Search food items via Open Food Facts API
- Results cached locally — previously searched items load instantly
- Manual food item creation for custom entries
- Macro calculations based on quantity in grams

**Body metrics**
- Log dated body measurements — weight, body fat, muscle mass, and tape measurements
- View full measurement history with expandable entries

**Dashboard**
- Overview of recent workouts, personal records, and latest body metrics
- Quick navigation to all features

---

## Project structure

```
JacksFitnessApp/
├── JacksFitnessApp.Domain/          # Entities, enums, repository interfaces
├── JacksFitnessApp.Application/     # CQRS queries, commands, handlers, DTOs, mappings
├── JacksFitnessApp.Infrastructure/  # EF Core DbContext, repositories, OpenFoodFacts service
├── JacksFitnessApp.Host/            # Minimal API endpoints, JWT auth, middleware, Program.cs
├── JacksFitnessApp.Tests/           # Test project (unit tests out of scope for this project)
└── fitnessapp-ui/                   # Angular 21 frontend
```

---

## Architecture

The backend follows Clean Architecture with strict dependency rules:

```
Domain ← no external dependencies
Application ← depends on Domain only
Infrastructure ← depends on Application and Domain
Host ← depends on everything, wires it all together
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

## Domain model

**Training context**
- `Exercise` — master exercise library with factory methods for global and custom exercises
- `Programme` → `ProgrammeWeek` → `ProgrammeDay` → `PlannedExercise` — training plan hierarchy
- `WorkoutSession` → `WorkoutSet` / `CardioSet` — logged training data

**Nutrition context**
- `NutritionLog` → `Meal` → `MealItem` → `FoodItem` — daily food intake hierarchy
- Macro calculations are computed properties on `MealItem` based on quantity and per-100g values

**Metrics context**
- `BodyMetric` — dated body measurement snapshots with nullable fields

---

## Setup

### Prerequisites

- .NET 10 SDK
- Node.js 18+
- SQL Server LocalDB (installed with Visual Studio)

### Backend setup

```bash
cd JacksFitnessApp
dotnet restore
dotnet run --project JacksFitnessApp.Host
```

The API runs on `https://localhost:7136`. On first run EF Core migrations execute automatically and the exercise library is seeded.

Swagger UI is available at `https://localhost:7136/swagger`.

### Frontend setup

```bash
cd fitnessapp-ui
npm install
ng serve
```

The app runs on `http://localhost:4200`.

### First use

Register a new account via the Register tab on the login page. Default API port is `7136` — if yours differs update `src/app/core/config/api.config.ts`.

---

## Key design decisions

**Personal record detection** — calculated on the fly at the point of logging rather than stored separately. Each new set is compared against the current best for that exercise and user. Simple, always accurate, no synchronisation required.

**Food search cache-aside** — Open Food Facts results are saved to the local database on first search. Subsequent searches check locally first, falling back to the API only when needed. User-created food items and API-sourced items coexist in the same table with a `Source` enum distinguishing their origin.

**Separate strength and cardio set entities** — `WorkoutSet` and `CardioSet` are separate entities rather than a single table with nullable columns. A strength set captures reps and weight; a cardio set captures duration, distance, and heart rate. Forcing both into one table would require many nullable columns and obscure the domain model.

**Macro safety** — all nutrition values from Open Food Facts are sanitised (HTML stripped) and clamped to realistic ranges before saving. Calories are capped at 900 per 100g (pure fat), macronutrients at 100g per 100g.

---

## Known limitations

- Personal record flags are not retroactively updated if a PR set is deleted
- JWT tokens expire after 60 minutes — the app redirects to login on 401
- MediatR and AutoMapper are used under their development licence — a commercial licence is required for production use
- Unit tests are out of scope for this project — testing patterns are demonstrated in the TicketSystem project

---