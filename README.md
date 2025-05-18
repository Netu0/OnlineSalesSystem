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

# Passo a passo para executar o projeto

## 1. Clone o repositório
- git clone https://github.com/Netu0/OnlineSalesSystem.git
- cd OnlineSalesSystem

## 2. Configuração do arquivo .env
É importante que este arquivo esteja na raiz do projeto

- DB_HOST=XXX
- DB_PORT=XXX
- DB_USER=XXX
- DB_PASSWORD=XXX
- DB_NAME=XXX
- APP_PORT=XXX

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

## 🧍 Customer
#### 1. Métodos
| Método | Rota                          | Descrição                      |
|--------|-------------------------------|--------------------------------|
| GET    | `/customers`                 | Listar todos os clientes       |
| GET    | `/customers/{id}`            | Obter cliente por ID           |
| POST   | `/customers`                 | Criar novo cliente             |
| PUT    | `/customers/{id}`            | Atualizar cliente              |
| DELETE | `/customers/{id}`            | Remover cliente                |
| GET    | `/customers/{id}/orders`     | Listar pedidos de um cliente   |

#### 2. Atributos obrigatórios e relacionados
| Método | Rota | Descrição | Corpo Esperado | Notas Importantes |
|:------:|:----:|:--------- |:--------------:|:-----------------:|
| GET | `/customers` | Listar todos os clientes | — | — |
| GET | `/customers/{id}` | Obter cliente por ID | — | — |
| POST | `/customers` | Criar novo cliente | `{ "name": (obrigatório), "email": (obrigatório), "phone": (opcional) }` | - **Email** deve ser um e-mail válido. <br> - **Name** não pode ser vazio. |
| PUT | `/customers/{id}` | Atualizar cliente existente | `{ "name": (obrigatório), "email": (obrigatório), "phone": (opcional) }` | - **Email** e **Name** continuam obrigatórios.<br> - Atualiza todos os campos do cliente. |
| DELETE | `/customers/{id}` | Remover cliente | — | - **Não permitido** excluir clientes que possuem pedidos vinculados. |
| GET | `/customers/{id}/orders` | Listar pedidos do cliente | — | — |

# 🧾 Order

#### 1. Métodos
| Método | Rota                                  | Descrição                       |
|--------|----------------------------------------|---------------------------------|
| GET    | `/orders`                             | Listar todos os pedidos         |
| GET    | `/orders/{id}`                        | Obter pedido por ID             |
| GET    | `/orders/by-customer/{customerId}`    | Listar pedidos por cliente      |
| POST   | `/orders`                             | Criar novo pedido               |
| PUT    | `/orders/{id}`                        | Atualizar pedido                |
| DELETE | `/orders/{id}`                        | Remover pedido                  |


#### 2. Atributos obrigatórios e relacionados
| Método | Rota | Descrição | Corpo Esperado | Notas Importantes |
|:------:|:----:|:--------- |:--------------:|:-----------------:|
| GET | `/orders` | Listar todos os pedidos | — | — |
| GET | `/orders/{id}` | Obter pedido por ID | — | — |
| GET | `/orders/by-customer/{customerId}` | Listar pedidos de um cliente | — | — |
| POST | `/orders` | Criar novo pedido | `{ "customerId": (obrigatório), "orderDate": (obrigatório), "total": (obrigatório) }` | - **orderDate** deve ser uma data válida.<br> - **total** deve ser decimal positivo.<br> - **customerId** precisa existir no sistema. |
| PUT | `/orders/{id}` | Atualizar pedido existente | `{ "customerId": (obrigatório), "orderDate": (obrigatório), "total": (obrigatório) }` | - Mesmas validações do POST. |
| DELETE | `/orders/{id}` | Remover pedido | — | — |


Para detelhas completos e exemplos de requisição/resposta, utilize a interface Swagger.

## Json Web Token
O JWT é um método seguro e compacto de transmitir inforamções entre duas partes, normalmente entre cliente e um servidor. Basicamente, é composto por uma string compacta e segura para URLs que contém informações em formato de JSON, como dados de usuários, e é assinado para garantir sua autencidade e integridade.
Atualmente, esta aplicação contém implementada a validação JWT, criando a diferenciação de níveis de acesso de um usuário cliente e administrador. Essa mudança trás a possibilidade futura de implementação de funcionalidades em que apenas administradores podem acessar, limitando os usuários normais e garantindo a segurança da aplicação através de conteúdos protegidos.

## Configurações do Docker para fazer conteiners da aplicação
- `OnlineSalesSystem.Api/`: API principal
- `OnlineSalesSystem.Core/`: Interfaces e serviços de domínio
- `OnlineSalesSystem.Infrastructure/`: Repositórios e contexto de banco de dados

## 🐳 Como Executar com Docker
1. Certifique-se de ter [Docker](https://www.docker.com/) instalado.
2. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/OnlineSalesSystem.git
   cd OnlineSalesSystem
3. Execute a aplicação `docker-compose up --build`

## ✅ Observações Importantes

- A documentação da API está disponível via Swagger: ([http://localhost:5115/swagger/index.html](http://localhost:5000/swagger)
- Não é permitido excluir clientes com pedidos vinculados
