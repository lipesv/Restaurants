#!/bin/bash

set -e

# ==============================
# CORES
# ==============================
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

# ==============================
# CONFIG
# ==============================
SOLUTION_ROOT="/home/felvieir/source/repos/Restaurants"

INFRA_PROJECT="./src/Restaurants.Infrastructure/Restaurants.Infrastructure.csproj"
API_PROJECT="./src/Restaurants.API/Restaurants.API.csproj"

DB_CONTAINER_NAME="restaurants-db"

cd "$SOLUTION_ROOT"

# garantir dotnet-ef no PATH
export PATH="$PATH:$HOME/.dotnet/tools"

echo -e "${GREEN}===> Iniciando ambiente...${NC}"

# ==============================
# VALIDAR CAMINHOS
# ==============================
if [ ! -f "$INFRA_PROJECT" ]; then
  echo -e "${RED}Projeto não encontrado: $INFRA_PROJECT${NC}"
  exit 1
fi

if [ ! -f "$API_PROJECT" ]; then
  echo -e "${RED}Projeto não encontrado: $API_PROJECT${NC}"
  exit 1
fi

# ==============================
# SUBIR CONTAINER
# ==============================
echo -e "${GREEN}===> Subindo SQL Server...${NC}"
docker compose up -d database

# ==============================
# AGUARDAR SQL SERVER
# ==============================
echo -e "${GREEN}===> Aguardando SQL Server iniciar...${NC}"

until docker logs "$DB_CONTAINER_NAME" 2>&1 | grep -q "ready for client connections"
do
  echo -e "${YELLOW}Aguardando banco iniciar...${NC}"
  sleep 2
done

echo -e "${GREEN}===> SQL Server pronto!${NC}"

# ==============================
# MIGRATIONS (IDEMPOTENTE)
# ==============================
echo -e "${GREEN}===> Aplicando migrations (se necessário)...${NC}"

dotnet ef database update \
  --project "$INFRA_PROJECT" \
  --startup-project "$API_PROJECT"

echo -e "${GREEN}===> Banco atualizado com sucesso!${NC}"
echo -e "${GREEN}===> Ambiente pronto 🚀${NC}"