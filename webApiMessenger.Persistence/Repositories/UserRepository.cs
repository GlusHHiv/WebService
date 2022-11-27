using Microsoft.EntityFrameworkCore;
using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Persistence.Repositories;

public class UserRepository
{
    private readonly IDbContext _dbContext;

    public UserRepository(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddUser(User user)
    {
        var findUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (findUser != null)
            throw new ArgumentException($"Пользователь с id {user.Id} существует!");
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task AddFriend(int user1id, int user2id) 
    {
        if (user1id == user2id) throw new ArgumentException("Нельзя добавлять себя в друзья!");

        var findUser1 = await _dbContext.Users.Include(user => user.Friends).FirstOrDefaultAsync(u => u.Id == user1id);
        if(findUser1 == null) throw new ArgumentException($"Пользователь с id {user1id} не существует!");
        
        var findUser2 = await _dbContext.Users.Include(user => user.Friends).FirstOrDefaultAsync(u => u.Id == user2id);
        if (findUser2 == null ) throw new ArgumentException($"Пользователь с id {user2id} не существует!");
        
        findUser1.Friends.Add(findUser2);
        findUser2.Friends.Add(findUser1);
        await _dbContext.SaveChangesAsync();
    }

    public List<User> GetUsers()
    {
        return _dbContext.Users.AsNoTracking().ToList();
    }

    public IEnumerable<User> GetFriends(int id)
    {
        var user = _dbContext.Users.Include(u => u.Friends).FirstOrDefault(u => u.Id == id);
        if (user == null) throw new ArgumentException($"Пользователь с id {id} не существует!");
        
        return user.Friends;
    }
}