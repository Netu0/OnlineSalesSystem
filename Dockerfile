# Etapa 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar arquivos do projeto
COPY OnlineSalesSystem.sln ./
COPY OnlineSalesSystem.Api/ OnlineSalesSystem.Api/
COPY OnlineSalesSystem.Core/ OnlineSalesSystem.Core/
COPY OnlineSalesSystem.Infrastructure/ OnlineSalesSystem.Infrastructure/
COPY OnlineSalesSystem.Api/appsettings*.json OnlineSalesSystem.Api/

# Restaurar pacotes e compilar
RUN dotnet restore
RUN dotnet publish OnlineSalesSystem.Api/OnlineSalesSystem.Api.csproj -c Release -o /publish

# Etapa 2: Runtime da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

# Expor porta
EXPOSE 5000
CMD ["dotnet", "OnlineSalesSystem.Api.dll"]
