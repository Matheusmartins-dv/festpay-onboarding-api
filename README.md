# Festpay Onboarding API

Este projeto é uma API construída com .NET 9, adotando a arquitetura **Vertical Slice**, o sistema fornece suporte a operações basicas relacionadas a Contas e Transações financeiras.

## 🔧 Arquitetura

O projeto segue o padrão **Vertical Slice Architecture**, onde cada operação (slice) é isolada em termos de lógica, comandos, manipuladores, validações e endpoint próprio. Essa abordagem permite que cada recurso seja desenvolvido e evoluído de forma independente, promovendo coesão e separação de responsabilidades, de forma que regras de negócios continuem encapsuladas em objetos de domínio chamados entidades e a logica da aplicação continue em usecases

---

## 📦 Funcionalidades

### 🏦 Accounts (Contas)

- **Criar Conta**
  - Criação de contas no sistema.
  - Endpoint: `POST /api/v1/accounts`

- **Depositar Dinheiro ou Sacar Dinheiro**
  - Permite adicionar saldo a uma conta existente.
  - Endpoint: `PATCH /api/v1/accounts/{id}/balance`

- **Habilitar/Desabilitar Conta**
  - Permite reativar ou desativar uma conta.
  - Endpoint: `PATCH /api/v1/accounts/{id}`

- **Recuperar Contas**
  - Permiti vizualizar todas as contas registradas.
  - Endpoint: `GET /api/v1/accounts`

---

### 💳 Transactions (Transações)

- **Criar Transação**
  - Endpoint: `POST /api/v1/transactions`
  - Cria uma nova transação entre contas.

- **Listar Transações**
  - Endpoint: `GET /api/v1/transactions`
  - Lista todas as transações registradas.

- **Cancelar Transãção**
  - Endpoint: `POST /api/v1/transactions/cancel`
  - Cancela uma transação.

- **Encontrar uma Transação**
  - Endpoint: `GET /api/v1/transactions/{idTransaction}/accounts/{IdAccount}`
  - Permiti encontrar uma transação específica.

---

## 📁 Organização do Projeto

- `Festpay.Onboarding.Domain` → Entidades de domínio
- `Festpay.Onboarding.Infra` → Persistência de dados, Migrations e EF Core
- `Festpay.Onboarding.Application` → Funcionalidades separadas por entidades, cada recurso implementa um validador, usecase(handler) e endpoint

---

## 📌 Considerações

Este projeto segue boas práticas de DDD (Domain-Driven Design) e Clean Architecture onde aplicável, utilizando o EF Core como ORM e SQLite como banco de dados no ambiente local.

---
