namespace api.Data;

using api.Model.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ProfileDb> Profiles { get; set; }
    public DbSet<HortaDb> Hortas { get; set; }
    public DbSet<MembroDb> Membros { get; set; }
    public DbSet<EspecieDb> Especies { get; set; }
    public DbSet<PlantioDb> Plantios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProfileDb>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(CURRENT_DATE)");
        });

        modelBuilder.Entity<HortaDb>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(CURRENT_DATE)");
        });

        modelBuilder.Entity<PlantioDb>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(CURRENT_DATE)");
        });

        modelBuilder.Entity<MembroDb>(entity =>
        {
            entity.HasOne(m => m.Horta)
                .WithMany()
                .HasForeignKey(m => m.HortaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Profile)
                .WithMany()
                .HasForeignKey(m => m.PerfilId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EspecieDb>(entity =>
        {
            entity.Property(e => e.Nome).IsRequired();
            entity.Property(e => e.DiasParaRegar).IsRequired();
        });

        modelBuilder.Entity<PlantioDb>(entity =>
        {
            entity.HasOne(p => p.Especie)
                .WithMany()
                .HasForeignKey(p => p.EspecieId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Horta)
                .WithMany()
                .HasForeignKey(p => p.HortaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}