using System.ComponentModel.DataAnnotations;

namespace webApiMessenger.WebApi.Services;

public class JwtSettings
{
    public static string ConfigName = nameof(JwtSettings);

    [Required]
    public string ValidIssuer { get; set; }

    [Required]
    public string ValidAudience { get; set; }
    
    [Required]
    public string IssuerSigningKey { get; set; }

    [Required]
    public string Secret { get; set; }

    [Required]
    public int ExpiresDay { get; set; }
}