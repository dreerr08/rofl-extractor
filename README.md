# Extrator ROFL - League of Legends Replay Converter

![LoL Esports](https://assets.lolesports.com/watch/static/media/lol-logo.ee873fc4.png)

Uma aplicação web moderna para converter arquivos ROFL (replays do League of Legends) em formato JSON, permitindo fácil análise e compartilhamento de dados de partidas.

## 🚀 Funcionalidades

- Upload de arquivos ROFL via interface web
- Conversão automática para JSON
- Interface moderna e responsiva
- Suporte para drag-and-drop
- Processamento rápido e eficiente
- Download automático do JSON gerado

## 💻 Tecnologias Utilizadas

- **Backend**:
  - ASP.NET Core 9.0
  - C#
  - Razor Pages
  - Fraxiinus.Rofl.Extract.Data (biblioteca para extração de dados ROFL)

- **Frontend**:
  - HTML5
  - CSS3 (com efeitos modernos)
  - JavaScript
  - Bootstrap 5
  - Google Fonts

## 🛠️ Como Usar

1. Acesse a aplicação em [URL_DO_RAILWAY]
2. Clique no botão de upload ou arraste um arquivo ROFL
3. Aguarde o processamento
4. O arquivo JSON será baixado automaticamente

## 📦 Dados Extraídos

O JSON gerado contém informações detalhadas da partida, incluindo:
- Metadados da partida
- Estatísticas dos jogadores
- Informações do jogo
- Dados técnicos do replay

## 🔧 Desenvolvimento Local

Para rodar o projeto localmente:

```bash
# Clone o repositório
git clone https://github.com/dreerr08/rofl-extractor.git

# Entre no diretório
cd rofl-extractor

# Restaure as dependências
dotnet restore

# Execute o projeto
dotnet run --project RoflWebExtractor/RoflWebExtractor.csproj
```

## 🌐 Requisitos

- .NET 9.0 SDK ou superior
- Navegador web moderno
- Conexão com internet

## 📝 Estrutura do Projeto

```
RoflWebExtractor/
├── Pages/                 # Páginas Razor
├── wwwroot/              # Arquivos estáticos
│   ├── css/             # Estilos CSS
│   ├── js/              # Scripts JavaScript
│   └── images/          # Imagens
├── Models/              # Classes de modelo
└── Program.cs          # Configuração da aplicação
```

## 🔒 Segurança

- Validação de tipos de arquivo
- Limpeza automática de arquivos temporários
- Proteção contra CSRF
- Sanitização de nomes de arquivo

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um Fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE.txt) para mais detalhes.

## 👥 Autor

- **dreerr08** - [GitHub](https://github.com/dreerr08)

## 🙏 Agradecimentos

- Riot Games pela API do League of Legends
- Comunidade LoL por feedback e sugestões
- Fraxiinus pela biblioteca de extração ROFL

---

Feito com ❤️ para a comunidade do League of Legends

