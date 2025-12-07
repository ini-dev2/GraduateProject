using CoreBoxBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreBoxBackend.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Statistic> Statistics => Set<Statistic>();
        public DbSet<Bundle> Bundles => Set<Bundle>();
        public DbSet<Image> Images => Set<Image>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Modules)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Statistics)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Module>()
                .HasMany(m => m.Statistics)
                .WithOne(s => s.Module)
                .HasForeignKey(s => s.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bundle>(b =>
            {
                b.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(100);

                b.Property(x => x.BundleUrl)
                 .IsRequired()
                 .HasMaxLength(255);

                b.Property(x => x.Version)
                 .HasMaxLength(20);

                b.Property(x => x.CreatedAt)
                 .IsRequired();

                b.Property(x => x.UpdatedAt)
                 .IsRequired();
            });

            modelBuilder.Entity<Image>(i =>
            {
                i.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(100);

                i.Property(x => x.ImageUrl)
                 .IsRequired()
                 .HasMaxLength(255);

                i.Property(x => x.CreatedAt)
                 .IsRequired();

                i.Property(x => x.UpdatedAt)
                 .IsRequired();
            });
        }
    }
}
