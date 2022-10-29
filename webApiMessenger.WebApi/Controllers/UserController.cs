using Microsoft.AspNetCore.Mvc;
using webApiMessenger.Application.services;
using webApiMessenger.Domain.Entities;
using webApiMessenger.WebApi.DTOs;

namespace webApiMessenger.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : Controller
{
    private UserService _userService = new();
    [HttpPost]
    public void AddFriend (int user1id, int user2id) 
    { 
        _userService.AddFriend(user1id, user2id);
    }
    private RegistrationService _registrationService = new(); 
    
    [HttpPost]
    public void Register(RegistrationUserDTO registrationUserDto)
    {
        _registrationService.RegisterUser(
            registrationUserDto.Login, 
            registrationUserDto.Password, 
            registrationUserDto.Email, 
            registrationUserDto.Nick, 
            registrationUserDto.Age
            );
    }

    [HttpGet]
    public IEnumerable<User> GetUsers()
    {
        return _registrationService.GetUsers();
    }
}