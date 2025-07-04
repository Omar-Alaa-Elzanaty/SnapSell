# Stage 1: Restore & Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory
WORKDIR /src

# Copy solution and project files
COPY SnapSell.sln .

COPY SnapSell.API/*.csproj ./SnapSell.API/
COPY SnapSell.Application/*.csproj ./SnapSell.Application/
COPY SnapSell.Model/*.csproj ./SnapSell.Model/
COPY SnapSell.Infrastructure/*.csproj ./SnapSell.Infrastructure/
COPY SnapSell.Presistance/*.csproj ./SnapSell.Presistance/
COPY SnapSell.Presentation/*.csproj ./SnapSell.Presentation/


# Restore NuGet packages
RUN dotnet restore "SnapSell.API/SnapSell.API.csproj"

# Copy the entire source
COPY . .

# Build the project
WORKDIR /src/SnapSell.API
RUN dotnet build "SnapSell.API.csproj" -c Release --no-restore

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "SnapSell.API.csproj" -c Release --no-restore -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Set working directory in container
WORKDIR /app

# Copy published files from previous stage
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080

# Expose the API port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s \
  CMD curl -f http://localhost/health || exit 1

# Start the API
ENTRYPOINT ["dotnet", "SnapSell.API.dll"]
