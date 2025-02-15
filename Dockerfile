FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar os arquivos de projeto e restaurar dependências
COPY ["RoflWebExtractor/RoflWebExtractor.csproj", "RoflWebExtractor/"]
COPY ["Rofl.Extract.Data/Rofl.Extract.Data.csproj", "Rofl.Extract.Data/"]
RUN dotnet restore "RoflWebExtractor/RoflWebExtractor.csproj"

# Copiar todo o código fonte
COPY . .

# Publicar a aplicação
WORKDIR "/src/RoflWebExtractor"
RUN dotnet publish "RoflWebExtractor.csproj" -c Release -o /app/publish

# Build da imagem final
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Criar diretórios para uploads
RUN mkdir -p /app/Uploads/Rofl \
    && mkdir -p /app/Uploads/Json \
    && mkdir -p /app/Uploads/Logs \
    && chmod 777 /app/Uploads -R

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RoflWebExtractor.dll"] 