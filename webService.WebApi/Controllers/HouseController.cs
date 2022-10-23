using Microsoft.AspNetCore.Mvc;
using webService.WebApi.DTOs;

namespace webService.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HouseController : Controller
{
    public static List<House> Houses = new()
    {
        new House
        {
            Name = "DomBita",
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
    public List<House> GetHouses()
    {
        return Houses;
    }

    [HttpPost]
    public void CreateHouse([FromBody] House house)
    {
        Houses.Add(house);
    }

    [HttpPost]
    public IActionResult AddHumanToHouse([FromBody] HumanWithHouseDTO humanWithHouseDto)
    {
        var houseName = humanWithHouseDto.HouseName;
        var human = humanWithHouseDto.Human;
        
        var house = Houses.Find(house => house.Name == houseName);
        
        if (house == null)
            return NotFound();
        
        house.Humans.Add(human);
        return Ok();
    }
}