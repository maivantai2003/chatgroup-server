#!/bin/bash
set -e

echo "Waiting for SQL Server..."
sleep 10

echo "Running migrations..."
dotnet ef database update

echo "Starting application..."
dotnet chatgroup-server.dll
