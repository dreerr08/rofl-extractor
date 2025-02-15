using System.ComponentModel.DataAnnotations;

namespace RoflWebExtractor.Models;

public class ConvertedFile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string OriginalFileName { get; set; } = string.Empty;

    [Required]
    public string JsonFileName { get; set; } = string.Empty;

    public string JsonContent { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string? UserEmail { get; set; }

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 