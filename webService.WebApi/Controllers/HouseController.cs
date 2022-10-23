using Microsoft.AspNetCore.Mvc;

namespace webService.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HouseController : Controller {
    public static List<House> Houses = new()
    {
        new House
        { 
             HoName = "DomBita",
             Humans = new()
             {
                new Human
                { 
                    Name = "Петя",
                    Age = 92,
                    Gender = Gender.Helicopter   
                }
             }
        }
    };
    [HttpGet]
    public List<House> GetHumans()
    {
        return Houses;
    }

    [HttpPost]
    public void CreateHuman([FromBody] House house)
    {
        Houses.Add(house);
    }
}