using System.ComponentModel.DataAnnotations;

namespace MonkeyMcp.Core.Models
{
    public sealed class MonkeyServiceOptions
    {
        public const string SectionName = "MonkeyService";
    
        [Required]
        [Url]
        public string ApiUrl { get; set; } = string.Empty;
        
        public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromMinutes(15);
        
        [Range(1, 300)]
        public int HttpTimeoutSeconds { get; set; } = 30;
    }
}