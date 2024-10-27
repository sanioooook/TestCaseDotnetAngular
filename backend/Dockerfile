ARG FRONT_URL
# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the project file and restore dependencies
COPY ./WebAPI/WebAPI.csproj ./WebAPI/
COPY ./Entities/Entities.csproj ./Entities/
RUN dotnet restore ./WebAPI/WebAPI.csproj

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish ./WebAPI/WebAPI.csproj -c Release -o out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/out .

# Expose port 5000 for the API
EXPOSE 5000

# Start the application
ENTRYPOINT ["dotnet", "WebAPI.dll"]
