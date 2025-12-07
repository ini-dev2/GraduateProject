using System.ComponentModel.DataAnnotations;

namespace CoreBoxBackend.Entities
{
    public sealed class Image
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}