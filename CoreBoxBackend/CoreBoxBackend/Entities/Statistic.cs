using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBoxBackend.Entities
{
    public sealed class Statistic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
    }
}