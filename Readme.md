# ConcessionariaAPP

Este projeto é uma aplicação ASP.NET Core para gestão de concessionária, incluindo cadastro de clientes, veículos, vendas, fabricantes e dashboards com gráficos.

---

## Como rodar a aplicação

### 1. Rodando **SEM Docker** (localmente)

#### Pré-requisitos
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local ou remoto)
- Visual Studio ou VS Code

#### Passos

1. **Configurar o banco de dados**
   - Edite o arquivo `appsettings.Development.json` e configure a `ConnectionStrings:DefaultConnection` para apontar para seu SQL Server.
    ```json
        "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=ConcessionariaDb;Trusted_Connection=True;TrustServerCertificate=True;"
        }
    ```
2. **Restaurar packages**
    ```sh
    dotnet restore
    ```

2. **Aplicar as migrations**
   ```sh
   dotnet ef database update --project ConcessionariaAPP
   ```

3. **Rodar a aplicação**
   ```sh
   dotnet run --project ConcessionariaAPP
   ```
   - Acesse em [http://localhost:5175](http://localhost:5175).

---

### 2. Rodando **COM Docker**

#### Pré-requisitos
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [Wsl](https://learn.microsoft.com/pt-br/windows/wsl/install)

#### Passos

1. **Configurar variáveis de ambiente**
   - Crie um arquivo `.env` a partir do `.env_copy` na raiz do projeto:
     ```
     SA_PASSWORD=MinhaSenhaForte!ParaoDesaioIntelectah2025
     DOTNET_RUNNING_IN_CONTAINER=true
     ```

2. **As migrations são aplicadas automaticamente** ao iniciar o container da aplicação.

---

## Atividades e Estrutura do Projeto

### Atividades realizadas
- Cadastro, edição e exclusão de clientes, veículos, fabricantes, concessionárias e vendas.
- Dashboard com gráficos dinâmicos (por cliente, fabricante, tipo de veículo, concessionária e mês).
- Validações customizadas (ex: CPF).
- Máscaras dinâmicas para campos de formulário.
- Integração com API externa para validação de CEP.
- Suporte a execução local e via Docker.
- Controle de acesso por perfil (Admin, Manager, Seller) nas rotas e funcionalidades.
- Autenticação, autorização e gerenciamento de usuários com ASP.NET Core Identity.
- Validações de formato, obrigatoriedade e regras de negócio nos formulários

### Estrutura do Projeto

```
ConcessionariaAPP/
│
├── Application/         # Serviços de aplicação e regras de negócio
├── Controllers/         # Controllers MVC
├── Domain/              # Entidades, interfaces e validações de domínio
├── Infrastructure/      # DbContext, configurações de EF Core
├── Models/              # ViewModels para as views
├── Views/               # Views e partials (Razor)
├── wwwroot/             # Arquivos estáticos (JS, CSS, imagens)
├── docker-compose.yml   # Orquestração dos containers
├── appsettings.*.json   # Configurações de ambiente
└── ...                  # Outros arquivos e pastas
```

---
# Pontos a Melhorar

- **Filtro das entidades:**  
  Implementar filtros mais avançados e dinâmicos para todas as entidades, permitindo múltiplos critérios e busca inteligente.

- **Otimizar consultas:**  
  Refatorar queries para usar projeções, `Include` seletivo e paginação, reduzindo o consumo de memória e tempo de resposta.

- **Estilo da página:**  
  Melhorar o layout e responsividade, aplicar design mais moderno e intuitivo, padronizar componentes visuais.

- **Adicionar Relatórios:**  
  Criar telas e exportação de relatórios (PDF, Excel) para vendas, clientes, veículos, etc.

- **Melhorar validações:**  
  Centralizar e aprimorar validações de negócio, aplicar validações customizadas no domínio e feedback visual nos formulários.

- **Testes Unitários:**  
  Cobrir serviços, controllers e regras de negócio com testes unitários e de integração.

- **Cache:**  
  Implementar cache para consultas frequentes (ex: dashboards, listas de entidades) para melhorar performance.

- **Swagger:**  
  Adicionar documentação automática da API com Swagger/OpenAPI para facilitar integração e testes externos.

---


