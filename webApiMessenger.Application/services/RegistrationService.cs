using System.Collections.ObjectModel;
using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;
using webApiMessenger.Domain.Repositories;

namespace webApiMessenger.Application.services;

public class RegistrationService
{
    private UserRepository _userRepository;
    public RegistrationService(IDbContext dbContext)
    {
        _userRepository = new UserRepository(dbContext);
    }

    public void RegisterUser(string login, string password, string email, string nick, int age)
    {
        // ЯВЛЯЕТСЯ ПЛОХОЙ ПРАКТИКОЙ, ПОЛЬЗОВАТЕЛЬ КОТОРЫЙ ВЫЗВАЛ МЕТОД НЕ УЗНАЕТ КАКОЙ АРГУМЕНТ ЯВЛЯЕТСЯ NULL
        var validateStrings = new[] { login, password, email, nick };
        if (validateStrings.All(s => string.IsNullOrEmpty(s)))
            throw new ArgumentException("Поля должны быть не null");

        var newUser = new User
        {
            Age = age,
            Email = email,
            Nick = nick,
            Login = login,
            Password = password
        };
        _userRepository.AddUser(newUser);
    }

    public IEnumerable<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }
}