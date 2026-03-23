using LexicoAnalyzer.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LexicoAnalyzer.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReservedWord> ReservedWords => Set<ReservedWord>();
        public DbSet<Delimiter> Delimiters => Set<Delimiter>();
        public DbSet<OperatorSymbol> OperatorSymbols => Set<OperatorSymbol>();
        public DbSet<LexicalErrorCatalog> LexicalErrorCatalog => Set<LexicalErrorCatalog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReservedWord>(entity =>
            {
                entity.ToTable("ReservedWords");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Word).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Word).IsUnique();
            });

            modelBuilder.Entity<Delimiter>(entity =>
            {
                entity.ToTable("Delimiters");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.Symbol).IsUnique();
            });

            modelBuilder.Entity<OperatorSymbol>(entity =>
            {
                entity.ToTable("OperatorSymbols");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.Symbol).IsUnique();
            });

            modelBuilder.Entity<LexicalErrorCatalog>(entity =>
            {
                entity.ToTable("LexicalErrorCatalog");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(250);
                entity.HasIndex(e => e.Code).IsUnique();
            });
        }
    }
}