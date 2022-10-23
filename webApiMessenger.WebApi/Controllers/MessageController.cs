using Microsoft.AspNetCore.Mvc;

namespace webApiMessenger.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MessageController : Controller
{
    [HttpGet]
    public void GetMessages()
    {
        
    }
}