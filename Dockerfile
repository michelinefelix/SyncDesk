# Etapa 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia e restaura as dependências do projeto
COPY ["SyncDesk/SyncDesk.csproj", "SyncDesk/"]
RUN dotnet restore "SyncDesk/SyncDesk.csproj"

# Copia o restante dos arquivos e compila
COPY . .
WORKDIR "/src/SyncDesk"
RUN dotnet build "SyncDesk.csproj" -c Release -o /app/build

# Etapa 2: Publicação
FROM build AS publish
RUN dotnet publish "SyncDesk.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Imagem final de execução (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Define variáveis de ambiente do ASP.NET
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SyncDesk.dll"]