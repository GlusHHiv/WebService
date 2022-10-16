using Microsoft.AspNetCore.Mvc;

namespace webService.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HumanController : Controller
{
    public static List<Hobby> Hobbies = new()
    {
        new Hobby
        {
            Name = "Рисовать"
        },
        new Hobby
        {
            Name = "Водить"
        },
        new Hobby
        {
            Name = "Программировать"
        },
        new Hobby
        {
            Name = "Слушать музыку"
        }
    };
    
    public static List<Human> Humans = new()
    {
        new Human
        {
            Name = "Петя",
            Age = 20,
            Gender = Gender.Man,
            Hobbies = new List<Hobby>
            {
                Hobbies[Random.Shared.Next(Hobbies.Count)]
            }
        },
        new Human
        {
            Name = "Ваня",
            Age = 14,
            Gender = Gender.Man,
            Hobbies = new List<Hobby>
            {
                Hobbies[Random.Shared.Next(Hobbies.Count)]
            }
        }
    };
    
    [HttpGet]
    public List<Human> GetHumans()
    {
        return Humans;
    }

    [HttpPost]
    public void CreateHuman([FromBody] Human human)
    {
        Humans.Add(human);
    }
}