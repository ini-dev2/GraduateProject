using System.ComponentModel.DataAnnotations;

namespace CoreBoxBackend.DTOs
{
    public sealed class CreateImageDto
    {
        public string Name { get; set; } = null!;
        public IFormFile File { get; set; } = null!;
    }

    public sealed class ImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}