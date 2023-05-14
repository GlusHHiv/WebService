using Microsoft.EntityFrameworkCore;
using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Persistence;

public class ApplicationDbContext : DbContext, IDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<GroupChat> GroupChats { get; set; }
    public DbSet<Message> Messages { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options) 
    {
        Database.EnsureCreated();
        //Database.EnsureDeleted();
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasMany(m => m.ReadedBy)
            .WithMany(u => u.LastReadMessagesInGroupChat);
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SendMessages);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}