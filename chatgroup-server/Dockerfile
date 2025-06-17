# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY chatgroup-server/*.csproj ./chatgroup-server/
RUN dotnet restore

COPY chatgroup-server/. ./chatgroup-server/
WORKDIR /app/chatgroup-server
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/chatgroup-server/out ./

# Lấy biến PORT từ Render
ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT
EXPOSE 10000

ENTRYPOINT ["dotnet", "chatgroup-server.dll"]
