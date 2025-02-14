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

### 1. Cadastro de Limite

**POST** `/api/limite`

```json
{
  "cpf": "12345678900",
  "agencia": "0001",
  "conta": "123456",
  "limite": 1000.00
}
```

### 2. Consulta de Limite

**GET** `/api/limite/{cpf}`

### 3. Atualização de Limite

**PUT** `/api/limite/{cpf}`

```json
{
  "limite": 2000.00
}
```

### 4. Remoção de Registro

**DELETE** `/api/limite/{cpf}`

### 5. Validação de Transação PIX

**POST** `/api/transacao`

```json
{
  "cpf": "12345678900",
  "valor": 500.00
}
```

## Testes

Para rodar os testes unitários:

```sh
   dotnet test
```

## Considerações Finais

Este projeto foi desenvolvido como parte do processo seletivo do **BTG Pactual** para a vaga de **Desenvolvedor Full Stack - FraudSys**. O objetivo é demonstrar conhecimento em **.NET 8**, boas práticas de código e implementação de soluções escaláveis e seguras.

