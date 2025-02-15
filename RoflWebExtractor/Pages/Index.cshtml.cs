using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fraxiinus.Rofl.Extract.Data;
using Fraxiinus.Rofl.Extract.Data.Models;
using System.Text.Json;
using RoflWebExtractor.Data;
using RoflWebExtractor.Models;

namespace RoflWebExtractor.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly AppDbContext _context;

    public string? ErrorMessage { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment, AppDbContext context)
    {
        _logger = logger;
        _environment = environment;
        _context = context;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var file = Request.Form.Files["roflFile"];
            if (file == null || file.Length == 0)
            {
                ErrorMessage = "Por favor, selecione um arquivo ROFL.";
                return Page();
            }

            // Criar diretório para arquivos ROFL
            var roflPath = Path.Combine(_environment.ContentRootPath, "Uploads", "Rofl");
            Directory.CreateDirectory(roflPath);

            // Criar diretório para arquivos JSON
            var jsonPath = Path.Combine(_environment.ContentRootPath, "Uploads", "Json");
            Directory.CreateDirectory(jsonPath);

            // Gerar nome único para o arquivo
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var roflFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{timestamp}.rofl";
            var roflFilePath = Path.Combine(roflPath, roflFileName);

            // Salvar arquivo ROFL
            using (var stream = new FileStream(roflFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Configurar opções de extração
            var options = new ReplayReaderOptions
            {
                LoadPayload = true,
                Verbose = true
            };

            // Extrair dados do arquivo ROFL
            var result = await ReplayReader.ReadReplayAsync(roflFilePath, options);

            if (result.Type == ReplayType.Unknown || result.Result == null)
            {
                ErrorMessage = "Não foi possível ler o arquivo ROFL.";
                return Page();
            }

            // Converter para JSON
            string jsonContent;
            if (result.Type == ReplayType.ROFL)
            {
                var rofl = (ROFL)result.Result;
                jsonContent = JsonSerializer.Serialize(rofl, new JsonSerializerOptions { WriteIndented = true });
            }
            else
            {
                var rofl2 = (ROFL2)result.Result;
                jsonContent = JsonSerializer.Serialize(rofl2, new JsonSerializerOptions { WriteIndented = true });
            }

            // Salvar JSON
            var jsonFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{timestamp}.json";
            var jsonFilePath = Path.Combine(jsonPath, jsonFileName);
            await System.IO.File.WriteAllTextAsync(jsonFilePath, jsonContent);

            // Salvar no banco de dados
            var convertedFile = new ConvertedFile
            {
                OriginalFileName = file.FileName,
                JsonFileName = jsonFileName,
                JsonContent = jsonContent,
                FileSize = file.Length,
                UserEmail = User.Identity?.Name,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            _context.ConvertedFiles.Add(convertedFile);
            await _context.SaveChangesAsync();

            // Criar arquivo de log com informações extras
            var logPath = Path.Combine(_environment.ContentRootPath, "Uploads", "Logs");
            Directory.CreateDirectory(logPath);
            var logContent = $"Data: {DateTime.Now}\n" +
                           $"Arquivo Original: {file.FileName}\n" +
                           $"Tamanho: {file.Length} bytes\n" +
                           $"IP: {HttpContext.Connection.RemoteIpAddress}\n" +
                           $"Tipo: {result.Type}\n";
            await System.IO.File.AppendAllTextAsync(
                Path.Combine(logPath, "uploads.log"),
                logContent + "\n-------------------\n"
            );

            // Retornar o JSON como download
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonContent);
            return File(bytes, "application/json", jsonFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar arquivo ROFL");
            ErrorMessage = "Ocorreu um erro ao processar o arquivo: " + ex.Message;
            return Page();
        }
    }
}
