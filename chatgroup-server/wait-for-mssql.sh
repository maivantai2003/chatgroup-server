#!/bin/sh

host="$1"
port="$2"
shift 2
cmd="$@"

echo "⏳ Waiting for SQL Server at $host:$port..."

until nc -z "$host" "$port"; do
  echo "🔄 SQL Server is unavailable - retrying..."
  sleep 2
done

echo "✅ SQL Server is ready - starting app..."
exec $cmd
