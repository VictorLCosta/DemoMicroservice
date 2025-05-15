FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas o .csproj primeiro
COPY Catalog.Api/Catalog.Api.csproj Catalog.Api/

# Restaura as dependências
RUN dotnet restore Catalog.Api/Catalog.Api.csproj

# Copia o restante dos arquivos
COPY . .

# Publica o projeto
WORKDIR /src/Catalog.Api
RUN dotnet publish -c Release -o /app

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]