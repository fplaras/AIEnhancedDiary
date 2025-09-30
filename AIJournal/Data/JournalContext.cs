
using Microsoft.EntityFrameworkCore;
using AIJournal.Models;

namespace AIJournal.Data
{
    public class JournalContext : DbContext
    {
        public JournalContext(DbContextOptions<JournalContext> options) : base(options)
        {
        }

        public DbSet<JournalEntry> JournalEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JournalEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.Mood).HasMaxLength(50);
                entity.Property(e => e.Tags).HasMaxLength(500);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
