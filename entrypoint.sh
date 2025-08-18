#!/usr/bin/env bash
set -euo pipefail

DB_HOST="${DB_HOST:-db}"
DB_PORT="${DB_PORT:-1433}"

echo "Aguardando SQL em $DB_HOST:$DB_PORT..."
until (echo > /dev/tcp/$DB_HOST/$DB_PORT) >/dev/null 2>&1; do
  sleep 2
done

# opcional: export APPLY_MIGRATIONS=true para condicionar o Migrate()
# export APPLY_MIGRATIONS=true

echo "Iniciando aplicação..."
exec dotnet ConcessionariaAPP.dll