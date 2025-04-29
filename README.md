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

## 1. Clone o repositório
- git clone https://github.com/Netu0/OnlineSalesSystem.git
- cd sistema-vendas-online

## 2. Configuração do arquivo .env

- DB_HOST=db
- DB_PORT=5432
- DB_USER=postgres
- DB_PASSWORD=postgres
- DB_NAME=sales_db
- APP_PORT=5115

## 3. Configurar o arquivo .yml

#### ⚙️ Arquivo `docker-compose.yml`

```yaml
version: '3.8'

services:
  db:
    image: postgres:13
    restart: always
    environment:
      POSTGRES_USER: ${DB_USER}
      POSTGRES_PASSWORD: ${DB_PASSWORD}
      POSTGRES_DB: ${DB_NAME}
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  app:
    build: .
    ports:
      - "${APP_PORT}:5115"
    env_file:
      - .env
    depends_on:
      - db

volumes:
  postgres_data:
```

---

## 4. Subir os containers com Docker Compose
- docker-compose up --build

## 5. Acesse a documentação da API via Swagger
A documentação Swagger permite explorar e testar todos os endpoints da aplicação diretamente pelo navegador.
- 🔗 Acesse: http://localhost:5115/swagger/index.html

# 🧪 Testando os Endpoints com Swagger
A interface do Swagger fornece:
- Listagem de todos os endpoints agrupados por entidade
- Testes interativos de requisições (GET, POST, PUT, DELETE)
- Visualização de parâmetros esperados, respostas e erros comuns
  - ✅ Exemplo: Para listar todos os clientes:
  - Navegue até GET /customers no Swagger
  - Clique em "Try it out"
  - Execute e veja a resposta da API diretamente!

## Customer

| Método | Rota                          | Descrição                      |
|--------|-------------------------------|--------------------------------|
| GET    | `/customers`                 | Listar todos os clientes       |
| GET    | `/customers/{id}`            | Obter cliente por ID           |
| POST   | `/customers`                 | Criar novo cliente             |
| PUT    | `/customers/{id}`            | Atualizar cliente              |
| DELETE | `/customers/{id}`            | Remover cliente                |
| GET    | `/customers/{id}/orders`     | Listar pedidos de um cliente   |

# Order

| Método | Rota                                  | Descrição                       |
|--------|----------------------------------------|---------------------------------|
| GET    | `/orders`                             | Listar todos os pedidos         |
| GET    | `/orders/{id}`                        | Obter pedido por ID             |
| GET    | `/orders/by-customer/{customerId}`    | Listar pedidos por cliente      |
| POST   | `/orders`                             | Criar novo pedido               |
| PUT    | `/orders/{id}`                        | Atualizar pedido                |
| DELETE | `/orders/{id}`                        | Remover pedido                  |


Para detelhas completos e exemplos de requisição/resposta, utilize a interface Swagger.

## ✅ Observações Importantes

- A documentação da API está disponível via Swagger: (http://localhost:5115/swagger/index.html)
- Não é permitido excluir clientes com pedidos vinculados.
- Validação de e-mail e telefone é obrigatória.
- O campo `total` nos pedidos deve conter valor decimal válido.
- Expansões futuras podem incluir produtos, itens do pedido e status do pedido.
- A versão atual da API pode conter inconsistências relacionadas a ORDER.
