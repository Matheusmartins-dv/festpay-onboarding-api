# Festpay Onboarding API

Este projeto é uma API construída com .NET 9, adotando a arquitetura **Vertical Slice**, o sistema fornece suporte a operações basicas relacionadas a Contas e Transações financeiras.

## 🔧 Arquitetura

O projeto segue o padrão **Vertical Slice Architecture**, onde cada operação (slice) é isolada em termos de lógica, comandos, manipuladores, validações e endpoint próprio. Essa abordagem permite que cada recurso seja desenvolvido e evoluído de forma independente, promovendo coesão e separação de responsabilidades, de forma que regras de negócios continuem encapsuladas em objetos de domínio chamados entidades e a logica da aplicação continue em usecases

---

## 📦 Funcionalidades

### 🏦 Accounts (Contas)

- **Criar Conta**
  - Criação de contas no sistema.

- **Depositar Dinheiro**
  - Endpoint: `POST /accounts/deposit`
  - Permite adicionar saldo a uma conta existente.

- **Remover Dinheiro**
  - Endpoint: `POST /accounts/remove`
  - Permite remover saldo de uma conta existente (saque).

- **Habilitar Conta**
  - Permite reativar uma conta desabilitada.

- **Desabilitar Conta**
  - Permite desativar uma conta para impedir movimentações.

---

### 💳 Transactions (Transações)

- **Criar Transação**
  - Endpoint: `POST /transactions`
  - Cria uma nova transação entre contas.

- **Listar Transações**
  - Endpoint: `GET /transactions`
  - Lista todas as transações registradas.

- **Recuperar Transação por ID**
  - Endpoint: `GET /transactions/{id}`
  - Retorna os detalhes de uma transação específica.

- **Cancelar Transação**
  - Endpoint: `POST /transactions/cancel`
  - Cancela uma transação existente, se elegível.

---

## 📁 Organização do Projeto

- `Festpay.Onboarding.Domain` → Entidades de domínio
- `Festpay.Onboarding.Infra` → Persistência de dados, Migrations e EF Core
- `Festpay.Onboarding.Application` → Funcionalidades separadas por entidades, cada recurso implementa um validador, usecase(handler) e endpoint

---

## 📌 Considerações

Este projeto está em desenvolvimento contínuo e segue boas práticas de DDD (Domain-Driven Design) e Clean Architecture onde aplicável, utilizando o EF Core como ORM e SQLite como banco de dados no ambiente local.

---
