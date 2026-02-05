# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["SalesTracker.sln", "./"]

# Copy project files
COPY ["SalesTracker.Web/SalesTracker.Web.csproj", "SalesTracker.Web/"]
COPY ["SalesTracker.Core/SalesTracker.Core.csproj", "SalesTracker.Core/"]

# Restore dependencies
RUN dotnet restore "SalesTracker.Web/SalesTracker.Web.csproj"

# Copy everything else
COPY . .

# Build the application
WORKDIR "/src/SalesTracker.Web"
RUN dotnet build "SalesTracker.Web.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "SalesTracker.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for health checks (optional but recommended)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=publish /app/publish .

# Expose port (Render will override this with $PORT)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "SalesTracker.Web.dll"]
