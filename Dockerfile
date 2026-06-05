# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY JacksFitnessApp.slnx .
COPY JacksFitnessApp.Domain/JacksFitnessApp.Domain.csproj JacksFitnessApp.Domain/
COPY JacksFitnessApp.Application/JacksFitnessApp.Application.csproj JacksFitnessApp.Application/
COPY JacksFitnessApp.Infrastructure/JacksFitnessApp.Infrastructure.csproj JacksFitnessApp.Infrastructure/
COPY JacksFitnessApp.Host/JacksFitnessApp.Host.csproj JacksFitnessApp.Host/
COPY JacksFitnessApp.Tests/JacksFitnessApp.Tests.csproj JacksFitnessApp.Tests/

# Restore dependencies
RUN dotnet restore JacksFitnessApp.slnx

# Copy everything else and build
COPY . .
RUN dotnet publish JacksFitnessApp.Host/JacksFitnessApp.Host.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "JacksFitnessApp.Host.dll"]