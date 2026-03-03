#!/bin/bash

# Cores para o terminal
GREEN='\033[0;32m'
NC='\033[0m' # Sem cor

echo -e "${GREEN}===> Iniciando ambiente de desenvolvimento (Zorin OS)...${NC}"

# 1. Sobe apenas o banco de dados em segundo plano
echo -e "${GREEN}===> Subindo SQL Server no Docker...${NC}"
docker compose up -d database

# 2. Aguarda o SQL Server ficar pronto (opcional, mas evita erros de conexão iniciais)
echo -e "${GREEN}===> Aguardando inicialização do banco...${NC}"
sleep 10

# 3. Executa as migrations para garantir que o banco está atualizado
echo -e "${GREEN}===> Aplicando migrations...${NC}"

# Usamos o caminho absoluto para evitar o erro de 'command not found'
$HOME/.dotnet/tools/dotnet-ef database update --project Restaurants.Infrastructure --startup-project Restaurants.API

# 4. Inicia a API no modo "Watch" (Hot Reload)
# Isso permite que você altere o código e a API reinicie sozinha
# echo -e "${GREEN}===> Iniciando API com Hot Reload (dotnet watch)...${NC}"
# dotnet watch run --project Restaurants.API