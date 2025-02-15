var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// Criar diretórios necessários
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "Uploads");
var roflPath = Path.Combine(uploadsPath, "Rofl");
var jsonPath = Path.Combine(uploadsPath, "Json");
var logsPath = Path.Combine(uploadsPath, "Logs");

Directory.CreateDirectory(uploadsPath);
Directory.CreateDirectory(roflPath);
Directory.CreateDirectory(jsonPath);
Directory.CreateDirectory(logsPath);

app.Run();
