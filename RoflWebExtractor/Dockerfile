FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copiar apenas os arquivos necessários primeiro
COPY ["RoflWebExtractor.csproj", "./"]
RUN dotnet restore

# Copiar o resto dos arquivos
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# Build da imagem final
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app
COPY --from=build /app/publish .

# Criar diretórios para uploads
RUN mkdir -p /app/Uploads/Rofl \
    && mkdir -p /app/Uploads/Json \
    && mkdir -p /app/Uploads/Logs \
    && chmod 777 /app/Uploads -R

ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE 8080

ENTRYPOINT ["dotnet", "RoflWebExtractor.dll"] 