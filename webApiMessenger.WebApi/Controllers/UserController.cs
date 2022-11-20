using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webApiMessenger.Application.services;
using webApiMessenger.WebApi.DTOs;

namespace webApiMessenger.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class UserController : Controller
{
    private UserService _userService;
    private RegistrationService _registrationService; 

    public UserController(UserService userService, RegistrationService registrationService)
    {
        _userService = userService;
        _registrationService = registrationService;
    }

    [HttpPost]
    public void AddFriend(int user1id, int user2id) 
    { 
        _userService.AddFriend(user1id, user2id);
    }
    
    [HttpPost]
    [AllowAnonymous]
    public string Register(RegistrationUserDTO registrationUserDto)
    {
        var userId = _registrationService.RegisterUser(
            registrationUserDto.Login, 
            registrationUserDto.Password, 
            registrationUserDto.Email, 
            registrationUserDto.Nick, 
            registrationUserDto.Age
            );
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, registrationUserDto.Nick),
            new("Id", userId.ToString()),
            new("Age", registrationUserDto.Age.ToString())
        };
        var jwt = Autorize(claims);
        return jwt;
    }

    private string Autorize(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("78EFAA44-6515-4FCD-87DF-50A0D737D41D"));
        var jwt = new JwtSecurityToken(
            claims: claims,
            issuer: "WebMessangerApp",
            audience: "Client",
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(LoginDTO loginDto)
    {
        var user = _userService.GetUsers()
            .FirstOrDefault(
                u => u.Login == loginDto.Login && 
                     u.Password == loginDto.Password);

        if (user == null) return Unauthorized();
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Nick), 
            new("Id", user.Id.ToString()),
            new("Age", user.Age.ToString())
        };
        var token = Autorize(claims);
        return Ok(Results.Json(new {token}));
    }

    [HttpGet]
    public UserWithoutFriendsDTO Me()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id");
        var userNick = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
        var userAge = User.Claims.FirstOrDefault(claim => claim.Type == "Age");

        return new UserWithoutFriendsDTO
        {
            Age = int.Parse(userAge.Value),
            Id = int.Parse(userId.Value),
            Nick = userNick.Value
        };
    }

    [HttpGet]
    public IEnumerable<UserWithoutFriendsDTO> GetUsers()
    {
        var users = _userService.GetUsers();
        return users.Adapt<IEnumerable<UserWithoutFriendsDTO>>();
    }

    [HttpGet]
    public IEnumerable<UserWithoutFriendsDTO> GetFriends()
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        var userFriends = _userService.GetFriends(userId);
        return userFriends.Adapt<IEnumerable<UserWithoutFriendsDTO>>();
    }
}