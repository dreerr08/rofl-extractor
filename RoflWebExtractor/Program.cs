using Microsoft.EntityFrameworkCore;
using RoflWebExtractor.Data;
using RoflWebExtractor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireClaim("IsAdmin", "true"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();

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
app.UseAuthentication();
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

// Criar o banco de dados e aplicar as migrações
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Recriar o banco de dados
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    
    Console.WriteLine("Banco de dados recriado com sucesso!");
}

app.Run();
