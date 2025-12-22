# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["chatgroup-server.csproj", "./"]
RUN dotnet restore --disable-parallel

COPY . .
RUN dotnet publish -c Release -o /app/publish


# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install netcat to check SQL available
RUN apt-get update && \
    apt-get install -y netcat-openbsd && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Add wait script
COPY wait-for-mssql.sh /app/wait-for-mssql.sh
# Convert CRLF → LF AND add execution permission
RUN sed -i 's/\r$//' /app/wait-for-mssql.sh \
    && sed -i '1s/^\xEF\xBB\xBF//' /app/wait-for-mssql.sh \
    && chmod +x /app/wait-for-mssql.sh

ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_ENVIRONMENT=Production

EXPOSE 80

ENTRYPOINT ["/app/wait-for-mssql.sh", "sql-server", "1433", "dotnet", "chatgroup-server.dll"]
