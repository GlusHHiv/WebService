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

    public void AddUser(User user)
    {
        var findUser = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
        if (findUser != null)
            throw new ArgumentException($"Пользователь с id {user.Id} существует!");
        
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }
    
    public void AddFriend(int user1id, int user2id) 
    {
        var findUser1 = _dbContext.Users.Include(user => user.Friends).FirstOrDefault(u => u.Id == user1id);
        var findUser2 = _dbContext.Users.Include(user => user.Friends).FirstOrDefault(u => u.Id == user2id);
        if (findUser1 == findUser2 )
        {
            throw new ArgumentException($"Нельзя добавлять себя в друзья!");
        }
        if (findUser2 == null )
        {
            throw new ArgumentException($"Пользователь с id {user2id} не существует!");
        }
        findUser1.Friends.Add(findUser2);
        findUser2.Friends.Add(findUser1);
        _dbContext.SaveChanges();
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