using JacksFitnessApp.Application.Handlers.Training.Exercise;
using JacksFitnessApp.Application.Interfaces;
using JacksFitnessApp.Application.Mappings;
using JacksFitnessApp.Domain.Interfaces.Coach;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Host.Endpoints;
using JacksFitnessApp.Host.Endpoints.Metrics;
using JacksFitnessApp.Host.Endpoints.Nutrition;
using JacksFitnessApp.Host.Endpoints.Training;
using JacksFitnessApp.Host.Middleware;
using JacksFitnessApp.Infrastructure.Data;
using JacksFitnessApp.Infrastructure.Data.Seed;
using JacksFitnessApp.Infrastructure.Repositories.Coach;
using JacksFitnessApp.Infrastructure.Repositories.Metrics;
using JacksFitnessApp.Infrastructure.Repositories.Nutrition;
using JacksFitnessApp.Infrastructure.Repositories.Training;
using JacksFitnessApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<FitnessAppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        }
    ));


// ASP.NET Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<FitnessAppDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

// Coach repositories and services
builder.Services.AddScoped<ICoachRepository, CoachRepository>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpClient<AnthropicService>();
builder.Services.AddScoped<IAnthropicService, AnthropicService>();

// Repositories
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IProgrammeRepository, ProgrammeRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<INutritionRepository, NutritionRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IBodyMetricRepository, BodyMetricRepository>();

// Open Food Facts HTTP client
builder.Services.AddHttpClient<OpenFoodFactsService>();
builder.Services.AddScoped<IFoodSearchService, OpenFoodFactsService>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetExercisesHandler).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(typeof(FitnessProfile).Assembly);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://calm-smoke-0800bf803-preview.westeurope.7.azurestaticapps.net"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Run migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FitnessAppDbContext>();
    await context.Database.MigrateAsync();

    // Seed exercises if none exist
    if (!context.Exercises.Any())
    {
        var exercises = ExerciseSeed.GetExercises();
        context.Exercises.AddRange(exercises);
        await context.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

// Register endpoints
app.MapAuthEndpoints();
app.MapExerciseEndpoints();
app.MapWorkoutEndpoints();
app.MapProgrammeEndpoints();
app.MapNutritionEndpoints();
app.MapBodyMetricEndpoints();
app.MapCoachEndpoints();

app.Run();