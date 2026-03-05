#!/bin/bash

# Cores para o terminal
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # Sem cor

# Caminho para o executável do EF no WSL
EF_PATH="$HOME/.dotnet/tools/dotnet-ef"
# Caminho do arquivo de trava dentro do volume (mapeado localmente pelo Docker)
# Nota: Como o SQL Server roda como root no container, precisamos verificar 
# se o banco de dados já foi inicializado.
FLAG_FILE="./scripts/environment/.migrations_applied"

echo -e "${GREEN}===> Iniciando ambiente de desenvolvimento...${NC}"

# 1. Sobe apenas o banco de dados
echo -e "${GREEN}===> Subindo SQL Server no Docker...${NC}"
docker compose up -d database

# 2. Verifica se as migrations já foram aplicadas anteriormente
if [ -f "$FLAG_FILE" ]; then
    echo -e "${YELLOW}===> Banco de dados já inicializado anteriormente. Pulando migrations...${NC}"
else
    echo -e "${GREEN}===> Banco novo detectado. Aguardando inicialização do SQL Server...${NC}"
    # Aguarda o SQL Server estar pronto para aceitar conexões
    sleep 15 

    echo -e "${GREEN}===> Aplicando migrations pela primeira vez...${NC}"
    
    if $EF_PATH database update --project Restaurants.Infrastructure --startup-project Restaurants.API; then
        # Cria o arquivo de marcação se a migração for bem-sucedida
        touch "$FLAG_FILE"
        echo -e "${GREEN}===> Migrations aplicadas com sucesso!${NC}"
    else
        echo -e "${RED}===> Erro ao aplicar migrations. Verifique os logs.${NC}"
        exit 1
    fi
fi

echo -e "${GREEN}===> Infraestrutura pronta para o VS Code.${NC}"