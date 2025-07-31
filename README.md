# Jira-ya

## Guia de Configuração

Este documento auxilia na configuração e execução do projeto Jira-ya em ambiente de desenvolvimento.

### Pré-requisitos
- .NET SDK 6.0 ou superior (recomendado .NET 8 ou 9)
- SQLite (não é necessário instalar, apenas o .NET)
- Visual Studio, VS Code ou outro editor compatível

### Estrutura do Projeto
- **Jira-ya.Api**: API principal (ASP.NET Core)
- **Jira-ya.Application**: Serviços e DTOs
- **Jira-ya.Domain**: Entidades e interfaces de domínio
- **Jira-ya.Infrastructure**: Persistência (EF Core), notificações, autenticação

### Configuração Inicial
1. **Restaurar os pacotes NuGet**
   ```sh
   dotnet restore
   ```
2. **Configurar a Connection String**
   O projeto já está configurado para usar SQLite. O arquivo de banco será criado automaticamente como `TempDatabase.db` na pasta da API.
   
   Arquivo: `Jira-ya.Api/appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=TempDatabase.db"
   }
   ```
3. **Gerar e aplicar as migrations**
   Execute os comandos abaixo a partir da raiz da solução (onde está o arquivo `.sln`):
   ```sh
   dotnet tool install --global dotnet-ef # (se necessário)
   dotnet ef migrations add InitialCreate --project Jira-ya.Infrastructure --startup-project Jira-ya.Api
   dotnet ef database update --project Jira-ya.Infrastructure --startup-project Jira-ya.Api
   ```
4. **Executar a aplicação**
   ```sh
   dotnet run --project Jira-ya.Api
   ```
   A API estará disponível em `https://localhost:5001` ou `http://localhost:5000`.

### Observações
- Se alterar entidades, crie novas migrations e atualize o banco:
  ```sh
  dotnet ef migrations add NomeDaMigration --project Jira-ya.Infrastructure --startup-project Jira-ya.Api
  dotnet ef database update --project Jira-ya.Infrastructure --startup-project Jira-ya.Api
  ```
- O serviço de notificação é apenas um stub. Implemente conforme a necessidade.
- Para testes, utilize ferramentas como Postman ou Swagger (já habilitado em desenvolvimento).

### Dúvidas
Em caso de dúvidas, consulte a documentação do ASP.NET Core, Entity Framework Core ou abra uma issue no repositório.