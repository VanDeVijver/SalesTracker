FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SalesTracker.Web/SalesTracker.Web.csproj", "SalesTracker.Web/"]
COPY ["SalesTracker.Core/SalesTracker.Core.csproj", "SalesTracker.Core/"]
RUN dotnet restore "SalesTracker.Web/SalesTracker.Web.csproj"
COPY . .
WORKDIR "/src/SalesTracker.Web"
RUN dotnet build "SalesTracker.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesTracker.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install EF Core tools for migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "SalesTracker.Web.dll"]
