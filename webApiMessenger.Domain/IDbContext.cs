using Microsoft.EntityFrameworkCore;
using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Domain;

public interface IDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<GroupChat> GroupChats { get; set; }
    DbSet<Message> Messages { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}