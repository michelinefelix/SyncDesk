# 🎧 SyncDesk — Plataforma SaaS Multi-Tenant de Atendimento

O **SyncDesk** é um sistema de Help Desk e suporte em tempo real desenvolvido em ASP.NET Core MVC. Projetado sob a arquitetura **SaaS Multi-Tenant**, o sistema garante o isolamento completo de dados entre organizações clientes e fornece comunicação bidirecional instantânea via WebSockets com SignalR.

---

## 🛠️ Tecnologias Utilizadas

* **Framework Backend:** .NET 8 / ASP.NET Core MVC
* **Persistência de Dados:** Entity Framework Core & SQL Server 2022
* **Comunicação em Tempo Real:** SignalR (`AtendimentoHub`)
* **Interface & Estilização:** Razor Views, Bootstrap 5 & Bootstrap Icons
* **Infraestrutura & Conteinerização:** Docker & Docker Compose

---

## ⚡ Funcionalidades do Sistema

* **Isolamento Multi-Tenant:** Segregação nativa por `TenantId` em todas as consultas e operações do banco de dados.
* **Painel Administrativo (Dashboard):** Visão sintética de métricas operacionais (total de chamados, pendências na fila, atendimentos ativos e departamentos).
* **Gestão de Departamentos:** Controle de setores de atendimento (ex: Suporte Técnico, Financeiro).
* **Fila de Espera Dinâmica:** Listagem e roteamento automático de chamados aguardando distribuição.
* **Distribuição de Chamados:** Mecanismo para agentes assumirem tickets da fila.
* **Chat ao Vivo:** Comunicação instantânea via SignalR entre a interface do cliente e o painel do atendente.
* **Portal do Cliente:** Formulário público para abertura de novos protocolos de suporte e acompanhamento do atendimento.

---

## 🧩 Modelo de Domínio
                     ┌─────────────────────────┐
                     │         TENANT          │
                     │  (Empresa Contratante)  │
                     └────────────┬────────────┘
                                  │
       ┌──────────────────────────┼──────────────────────────┐
       │                          │                          │
       ▼                          ▼                          ▼
┌──────────────┐           ┌──────────────┐           ┌──────────────┐
│     USER     │           │  DEPARTMENT  │           │   CUSTOMER   │
│  (Atendente) │           │   (Setor)    │           │   (Cliente)  │
└──────┬───────┘           └──────┬───────┘           └──────┬───────┘
       │                          │                          │
       │ (Assume)                 │ (Categoriza)             │ (Abre)
       └───────────────────┐      │      ┌───────────────────┘
                           ▼      ▼      ▼
                        ┌───────────────────┐
                        │      TICKET       │
                        │    (Protocolo)    │
                        └─────────┬─────────┘
                                  │
                                  │ (Possui N)
                                  ▼
                        ┌───────────────────┐
                        │      MESSAGE      │
                        │     (Chat)        │
                        └───────────────────┘


| Entidade | Descrição |
| :--- | :--- |
| **`Tenant`** | Entidade pai que representa a empresa contratante do SaaS e garante o isolamento dos dados. |
| **`User`** | Colaborador interno da empresa com perfis de permissão (`Admin`, `Supervisor`, `Agente`). |
| **`Customer`** | Cliente externo que solicita atendimento na plataforma. |
| **`Department`** | Áreas funcionais internas responsáveis por resolver as solicitações. |
| **`Ticket`** | Registro do chamado de atendimento com status, histórico e prioridade. |
| **`Message`** | Registro de cada interação realizada dentro do chat do ticket. |

---

## 🚀 Como Executar o Projeto (Via Docker)

### Pré-requisitos
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (com Docker Compose ativo)
* [Git](https://git-scm.com/)

---

### Passo a Passo

   ```bash
   docker compose up --build -d
   ```
> ℹ️ **Nota:** O container da aplicação executará automaticamente as migrações do Entity Framework (`MigrateAsync`) e a carga inicial de dados (`SeedAsync`) assim que o banco SQL Server estiver totalmente inicializado e *healthy*.

---

### Conexão Externa para Debug (SSMS / Azure Data Studio):

* **Usuário:** sa
* **Senha:** A senha configurada no seu docker-compose.yml

## 📍 Swagger

* **Swagger:** `/swagger/index.html`

## 📍 Mapeamento de Rotas

| Funcionalidade | Rota / URL |
| :--- | :--- |
| **Portal do Cliente (Abertura/Chat)** | `http://localhost:5000/Customer` |
| **Painel de Atendimento (Tickets)** | `http://localhost:5000/Ticket` |
| **Fila de Espera (Agente)** | `http://localhost:5000/Ticket/Fila` |
| **Gestão de Departamentos** | `http://localhost:5000/Admin/Departamentos` |
| **Dashboard Administrativo** | `http://localhost:5000/Admin/Dashboard` |
| **Documentação da API (Swagger)** | `http://localhost:5000/swagger` |

---

## 🛠️ Comandos Úteis do Docker

* **Visualizar logs da aplicação Web:**
  ```bash
  docker compose logs -f syncdesk-web
  ```
* **Parar os containers:**
  ```bash
  docker compose down
  ```
* **Parar e limpar volumes (resetar banco de dados):**
  ```bash
  docker compose down -v
  ```

---

SyncDesk/
├── Controllers/                  <-- Controladores de fluxo e ações das telas
│   ├── AdminController.cs        <-- Gestão de dashboards e setores
│   ├── CustomerController.cs     <-- Portal de autoatendimento e chat do cliente
│   └── TicketController.cs       <-- Fila de espera, atendimento do agente e tickets
├── Data/                         <-- Persistência de dados com EF Core
│   ├── DbInitializer.cs          <-- Carga inicial de dados de teste (Seed)
│   └── SyncDeskDbContext.cs      <-- Mapeamento e contexto do SQL Server
├── Hubs/                         <-- Comunicação bidirecional WebSockets
│   └── AtendimentoHub.cs         <-- Endpoint do SignalR (`/atendimentoHub`)
├── Models/                       <-- Modelos de dados do sistema
│   ├── Entities/                 <-- Tenant, User, Customer, Department, Ticket, Message
│   └── Enums/                    <-- TicketStatusEnum, PrioridadeEnum, TipoRemetenteEnum, PerfilEnum
├── Services/                     <-- Regras de negócio da aplicação
│   ├── Implementations/          <-- CustomerService, DepartmentService, MessageService, TenantService, TicketService, UserService
│   └── Interfaces/               <-- ICustomerService, IDepartmentService, IMessageService, ITenantService, ITicketService, IUserService
├── Views/                        <-- Interface gráfica Razor (.cshtml)
│   ├── Admin/                    <-- Dashboard.cshtml, Departamentos.cshtml
│   ├── Customer/                 <-- Index.cshtml (Formulário), Chat.cshtml (Atendimento)
│   ├── Shared/                   <-- _Layout.cshtml, _ValidationScriptsPartial.cshtml
│   └── Ticket/                   <-- Index.cshtml, Fila.cshtml, Atender.cshtml
├── appsettings.json              <-- Configurações e string de conexão (DefaultConnection SQL Server)
└── Program.cs                    <-- Pipeline HTTP, DI, SignalR, Swagger e Mapeamento de Rotas