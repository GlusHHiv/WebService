using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webApiMessenger.Application.services;
using webApiMessenger.Domain.Abstractions;
using webApiMessenger.Domain.Entities;
using webApiMessenger.Shared.DTOs;


namespace webApiMessenger.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class UserController : Controller
{
    private UserService _userService;
    private RegistrationService _registrationService;
    private readonly ITokenProvider _tokenProvider;

    public UserController(UserService userService, RegistrationService registrationService, ITokenProvider tokenProvider)
    {
        _userService = userService;
        _registrationService = registrationService;
        _tokenProvider = tokenProvider;
    }

    [HttpPost]
    public async Task AddFriend(int user2Id)
    { 
        var user1Id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
        await _userService.AddFriend(user1Id, user2Id);
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<string> Register(RegistrationUserDTO registrationUserDto)
    {
        var userId = await _registrationService.RegisterUser(
            registrationUserDto.Login, 
            registrationUserDto.Password, 
            registrationUserDto.Email, 
            registrationUserDto.Nick, 
            registrationUserDto.Age
            );
        var jwt = await _tokenProvider.GetJwt(registrationUserDto.Login);
        return jwt;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        var users = await _userService.GetUsers();
        var user = users.FirstOrDefault(
                u => u.Login == loginDto.Login && 
                     u.Password == loginDto.Password);

        if (user == null) return Unauthorized();
        var jwt = await _tokenProvider.GetJwt(user.Login);
        return Ok(jwt);
    }

    [HttpGet]
    public UserPublicDTO Me()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id");
        var userNick = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
        var userAge = User.Claims.FirstOrDefault(claim => claim.Type == "Age");
        var userEmail = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
        var userLogin = User.Claims.FirstOrDefault(claim => claim.Type == "Login");

        return new UserPublicDTO
        {
            Age = int.Parse(userAge.Value),
            Id = int.Parse(userId.Value),
            Nick = userNick.Value,
            Email = userEmail.Value,
            Login = userLogin.Value
        };
    }

    [HttpGet]
    public async Task<IEnumerable<UserWithoutFriendsDTO>> GetUsers()
    {
        var users = await _userService.GetUsers();
        return users.Adapt<IEnumerable<UserWithoutFriendsDTO>>();
    }

    [HttpGet]
    public async Task<IEnumerable<UserWithoutFriendsDTO>> GetFriends()
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        var userFriends = await _userService.GetFriends(userId);
        return userFriends.Adapt<IEnumerable<UserWithoutFriendsDTO>>();
    }
}