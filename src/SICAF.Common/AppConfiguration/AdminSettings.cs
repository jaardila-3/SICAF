using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.AppConfiguration;

public class AdminSettings
{
    public const string SectionName = "AdminSettings";

    [Required]
    public required string Email { get; set; } = string.Empty;

    [Required]
    public required string Password { get; set; } = string.Empty;

    [Required]
    public required string Name { get; set; } = string.Empty;

    [Required]
    public required string LastName { get; set; } = string.Empty;

    [Required]
    public required string IdentificationNumber { get; set; } = string.Empty;

    [Required]
    public required string Username { get; set; } = string.Empty;

    public string? Dependency { get; set; }
}