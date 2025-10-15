#!/bin/bash
set -e

HOST="192.168.0.100" # Docker Compose'da tanımlanan PostgreSQL'in sabit IP'si
PORT="5432"

echo "Waiting for PostgreSQL to be ready at $HOST:$PORT..."

# PostgreSQL bağlantıları kabul edene kadar döngüde bekle
while ! pg_isready -h $HOST -p $PORT -U postgres; do
  echo "PostgreSQL is unavailable - sleeping for 1 second..."
  sleep 1
done

echo "PostgreSQL is up and running! Starting application..."

# Uygulamayı başlat
dotnet StockApp.Api.dll