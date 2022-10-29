using Microsoft.EntityFrameworkCore;
using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Persistence;

public class ApplicationDbContext : DbContext, IDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<GroupChat> GroupChats { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=webServiceMessenger.db");
        base.OnConfiguring(optionsBuilder);
    }
    
    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }
}