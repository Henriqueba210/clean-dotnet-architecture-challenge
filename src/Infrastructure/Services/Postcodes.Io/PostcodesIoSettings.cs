using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Services.Postcodes.Io;

public sealed class PostcodesIoSettings
{
    public const string ConfigurationSection = "PostcodesIo";

    [Required] [Url] public string BaseAddress { get; set; } = string.Empty;
}