namespace CoreBoxBackend.DTOs
{
    public sealed class ModuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string BundleUrl { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Version { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public sealed class CreateModuleDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string BundleUrl { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Version { get; set; } = null!;
    }

    public sealed class UpdateModuleDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? BundleUrl { get; set; }
        public string? Version { get; set; }
        public bool? IsActive { get; set; }
    }
}