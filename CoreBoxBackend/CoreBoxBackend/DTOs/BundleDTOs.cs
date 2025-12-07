using System.ComponentModel.DataAnnotations;

namespace CoreBoxBackend.DTOs
{
    public sealed class BundleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string BundleUrl { get; set; } = null!;
        public string? Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateBundleDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Version { get; set; } = null!;

        [Required]
        public IFormFile File { get; set; } = null!;
    }

    public sealed class UpdateBundleDto
    {
        public string? Name { get; set; }
        public string? BundleUrl { get; set; }
        public string? Version { get; set; }
    }
}