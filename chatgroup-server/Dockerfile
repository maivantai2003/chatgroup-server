# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["chatgroup-server.csproj", "./"]
#COPY ["nuget.config", "./"]
RUN dotnet restore  --disable-parallel

# Copy all source code and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_ENVIRONMENT=Production
EXPOSE 80
ENTRYPOINT ["dotnet", "chatgroup-server.dll"]
