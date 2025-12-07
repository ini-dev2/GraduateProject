using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBoxBackend.Entities
{
    public sealed class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [MaxLength(255)]
        public string BundleUrl { get; set; } = null!;

        [MaxLength(255)]
        public string ImageUrl { get; set; } = null!;

        [MaxLength(20)]
        public string Version { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
    }
}