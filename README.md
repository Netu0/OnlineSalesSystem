# 🛒 Sistema de Vendas Online

## 📋 Descrição
Projeto de um Sistema de Vendas Online, voltado para aplicações de e-commerce, gestão de pedidos e relacionamento com clientes. Implementa as entidades Customer e Order, possibilitando operações CRUD, consultas específicas e relacionamentos entre clientes e pedidos.

## Sistema de Vendas Online - API RESTful

API para gerenciamento de clientes e pedidos desenvolvida com ASP.NET Core, Entity Framework Core e PostgreSQL em containers Docker.

## 📋 Pré-requisitos

- Docker 24.0+
- Docker Compose 2.20+
- .NET SDK 7.0+
- IDE 

## 📦 Instruções de Execução com Docker
Requisitos de Software
- Docker: versão 20.10 ou superior

- Docker Compose: versão 1.29 ou superior

## Variáveis de Ambiente
Crie um arquivo .env na raiz do projeto.

# Passo a passo para executar o projeto

## Clone o repositório
- git clone https://github.com/Netu0/OnlineSalesSystem.git
- cd sistema-vendas-online

## Configuração do arquivo .env

- DB_HOST=db
- DB_PORT=5432
- DB_USER=postgres
- DB_PASSWORD=postgres
- DB_NAME=sales_db
- APP_PORT=5115

## Subir os containers com Docker Compose
- docker-compose up --build

## Acesse a documentação da API via Swagger
A documentação Swagger permite explorar e testar todos os endpoints da aplicação diretamente pelo navegador.
- 🔗 Acesse: http://localhost:5115/swagger/index.html




