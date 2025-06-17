# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY chatgroup-server.sln .
COPY chatgroup-server/chatgroup-server.csproj ./chatgroup-server/

# Restore dependencies
RUN dotnet restore chatgroup-server/chatgroup-server.csproj

# Copy source code
COPY chatgroup-server/. ./chatgroup-server/

# Build and publish
WORKDIR /app/chatgroup-server
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/chatgroup-server/out ./

# Configure port for Render
ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT
EXPOSE 10000

ENTRYPOINT ["dotnet", "chatgroup-server.dll"]
