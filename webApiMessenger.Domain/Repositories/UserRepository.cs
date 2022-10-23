using System.Collections.ObjectModel;
using webApiMessenger.Domain.Entities;

namespace webApiMessenger.Domain.Repositories;

public class UserRepository
{
    private static List<User> _users = new();
    public ReadOnlyCollection<User> Users => _users.AsReadOnly();

    static UserRepository()
    {
        var anton = new User
        {
            Id = 1,
            Nick = "Anton",
            Email = "anton@mail.ru",
            Age = 21,
            Login = "anton_21",
            Password = "123123123",
            Friends = new List<User>()
        };
        var roma = new User
        {
            Id = 2,
            Nick = "Roma",
            Email = "roma@mail.ru",
            Age = 22,
            Login = "roma",
            Password = "123123123",
            Friends = new List<User>
            {
                anton
            }
        };
        // anton.Friends.Add(roma);
        _users.Add(anton);
        _users.Add(roma);
    }

    public void AddUser(User user)
    {
        var findUser = _users.Find(u => u.Id == user.Id);
        if (findUser != null)
            throw new ArgumentException($"Пользователь с id {user.Id} существует!");
        
        _users.Add(user);
    }
}