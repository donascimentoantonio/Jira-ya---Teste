FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Jira-ya.Api/Jira-ya.Api.csproj", "Jira-ya.Api/"]
COPY ["Jira-ya.Application/Jira-ya.Application.csproj", "Jira-ya.Application/"]
COPY ["Jira-ya.Domain/Jira-ya.Domain.csproj", "Jira-ya.Domain/"]
COPY ["Jira-ya.Infrastructure/Jira-ya.Infrastructure.csproj", "Jira-ya.Infrastructure/"]
RUN dotnet restore "Jira-ya.Api/Jira-ya.Api.csproj"
COPY . .
WORKDIR "/src/Jira-ya.Api"
RUN dotnet build "Jira-ya.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jira-ya.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jira-ya.Api.dll"]
