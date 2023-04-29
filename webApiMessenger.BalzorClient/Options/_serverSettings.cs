using System.ComponentModel.DataAnnotations;

namespace webApiMessenger.BalzorClient.Options;

public class ServerSettings
{
    public static string ConfigName = "ServerSettings";

    [Required]
    public string Schema { get; set; }

    [Required]
    public string Host { get; set; }

    [Range(0, int.MaxValue)]
    public int? Port { get; set; }
    public string? BaseUrl { get; set; }

    public Uri Uri => new Uri(Schema + "://" + Host + (Port.HasValue ? $":{Port}" : string.Empty) + "/" + BaseUrl ??
                         string.Empty);
}