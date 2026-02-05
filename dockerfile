# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["SalesTracker.sln", "./"]
COPY ["SalesTracker.Web/SalesTracker.Web.csproj", "SalesTracker.Web/"]
COPY ["SalesTracker.Core/SalesTracker.Core.csproj", "SalesTracker.Core/"]

# Restore dependencies
RUN dotnet restore "SalesTracker.Web/SalesTracker.Web.csproj"

# Copy everything else
COPY . .

# Build the application
WORKDIR "/src/SalesTracker.Web"
RUN dotnet build "SalesTracker.Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SalesTracker.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Expose port
EXPOSE 80

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "SalesTracker.Web.dll"]
