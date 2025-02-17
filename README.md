# FraudSys - Gestão de Limite de Transações PIX

## Introdução

O **FraudSys** é um sistema desenvolvido para o **Banco KRT** com o objetivo de gerenciar os limites de transações PIX dos clientes. O sistema permite cadastrar, consultar, atualizar e remover limites, além de validar transações com base no saldo disponível.

## Tecnologias Utilizadas

- **.NET 8**
- **AWS DynamoDB** (banco de dados NoSQL)
- **Arquitetura MVC**
- **Princípios SOLID**
- **Clean Code**

## Estrutura do Projeto

```
fraudsys-limit-management/
│── src/                  # Código-fonte da aplicação
│   ├── Api/              # Controllers e Rotas
│   ├── Application/      # Casos de uso (Serviços)
│   ├── Domain/           # Modelos de domínio (Entidades e Regras)
│   ├── Infrastructure/   # Acesso a banco (DynamoDB)
│   ├── Tests/            # Testes Unitários
│── docs/                 # Documentação adicional
│── .gitignore            # Arquivos a serem ignorados pelo Git
│── README.md             # Documentação principal
│── FraudSys.sln          # Solução .NET
│── docker-compose.yml    # Arquivo Docker para facilitar execução
```

## Instalação e Execução

1. Clone o repositório:
   ```sh
   git clone https://github.com/seu-usuario/fraudsys-limit-management.git
   ```
2. Navegue até o diretório do projeto:
   ```sh
   cd fraudsys-limit-management
   ```
3. Instale as dependências do projeto:
   ```sh
   dotnet restore
   ```
4. Execute a aplicação:
   ```sh
   dotnet run --project src/Api
   ```

## Endpoints

### 1. Cadastro de Conta e Limite

**POST** `/api/account`

```json
{
  "document": "12345678900",
  "agency": "0001",
  "accountNumber": "123456",
  "pixLimit": 1000.00
}
```

### 2. Consulta de Conta e Limite

**GET** `/api/account/{accountNumber}`

### 3. Atualização de Limite

**PUT** `/api/account/{accountNumber}/limit`

```json
{
  "pixLimit": 2000.00
}
```

### 4. Remoção de Conta

**DELETE** `/api/account/{accountNumber}`

### 5. Processamento de Transação PIX

**POST** `/api/account/{accountNumber}/transaction`

```json
{
  "amount": 500.00
}
```
### 5. Processamento de Transação PIX

**POST** `/api/transaction/pix`

```json
{
  "accountNumber": "123456",
  "amount": 500.00
}

## Testes

Para rodar os testes unitários:

```sh
   dotnet test
```

## Considerações Finais

Este projeto foi desenvolvido como parte do processo seletivo do **BTG Pactual** para a vaga de **Desenvolvedor Full Stack - FraudSys**. O objetivo é demonstrar conhecimento em **.NET 8**, boas práticas de código e implementação de soluções escaláveis e seguras.
