using Microsoft.EntityFrameworkCore;

namespace EfCore.Cli;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=efcore.db");
        base.OnConfiguring(optionsBuilder);
    }

    public ApplicationDbContext()
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
        Database.AutoTransactionsEnabled = true;
    }
}