FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Expenses.csproj", "./"]
RUN dotnet restore

# Copy the rest of the code
COPY . .
RUN dotnet build "Expenses.csproj" -c Release -o /app/build
RUN dotnet publish "Expenses.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Expenses.dll"]