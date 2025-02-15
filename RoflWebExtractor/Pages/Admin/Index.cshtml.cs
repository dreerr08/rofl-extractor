using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoflWebExtractor.Data;
using RoflWebExtractor.Models;

namespace RoflWebExtractor.Pages.Admin;

[Authorize(Policy = "RequireAdmin")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public IndexModel(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public List<User> Users { get; set; } = new();
    public List<ConvertedFile> ConvertedFiles { get; set; } = new();

    public async Task OnGetAsync(string? orderBy)
    {
        Users = await _context.Users.OrderBy(u => u.Id).ToListAsync();

        var query = _context.ConvertedFiles.AsQueryable();

        query = orderBy switch
        {
            "size" => query.OrderByDescending(f => f.FileSize),
            "user" => query.OrderBy(f => f.UserEmail),
            _ => query.OrderByDescending(f => f.CreatedAt)
        };

        ConvertedFiles = await query.ToListAsync();
    }

    public async Task<IActionResult> OnPostMakeAdminAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.IsAdmin = true;
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveAdminAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.IsAdmin = false;
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteFileAsync(int fileId)
    {
        var file = await _context.ConvertedFiles.FindAsync(fileId);
        if (file != null)
        {
            // Excluir arquivo físico
            var jsonPath = Path.Combine(_environment.ContentRootPath, "Uploads", "Json", file.JsonFileName);
            if (System.IO.File.Exists(jsonPath))
            {
                System.IO.File.Delete(jsonPath);
            }

            _context.ConvertedFiles.Remove(file);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnGetDownloadAsync(int id)
    {
        var file = await _context.ConvertedFiles.FindAsync(id);
        if (file == null)
        {
            return NotFound();
        }

        var jsonPath = Path.Combine(_environment.ContentRootPath, "Uploads", "Json", file.JsonFileName);
        if (!System.IO.File.Exists(jsonPath))
        {
            return NotFound();
        }

        var jsonContent = await System.IO.File.ReadAllBytesAsync(jsonPath);
        return File(jsonContent, "application/json", file.JsonFileName);
    }
} 