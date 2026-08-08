using Microsoft.EntityFrameworkCore;
using IutKanoon.TelegramBot.Infrastructure.Data.Entities;


namespace IutKanoon.TelegramBot.Infrastructure.Data;

public class BotDbContext : DbContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
    }

    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<UserState> UserStates => Set<UserState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UserSession Configuration
        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.TelegramId);
            entity.Property(e => e.TelegramId).ValueGeneratedNever();
            entity.Property(e => e.UserEmail).HasMaxLength(255);
            entity.Property(e => e.UserRole).HasMaxLength(50);
        });

        // UserState Configuration
        modelBuilder.Entity<UserState>(entity =>
        {
            entity.HasKey(e => e.TelegramId);
            entity.Property(e => e.TelegramId).ValueGeneratedNever();
            entity.Property(e => e.CurrentState).HasMaxLength(100);
        });
    }
}
