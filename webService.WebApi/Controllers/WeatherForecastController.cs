using Microsoft.AspNetCore.Mvc;

namespace webService.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", 
        "Bracing", 
        "Chilly", 
        "Cool", 
        "Mild", 
        "Warm", 
        "Balmy", 
        "Hot", 
        "Sweltering", 
        "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        int count = 5;
        var weathers = new List<WeatherForecast>();
        for (int i = 0; i < count; i++)
        {
            var forecast = new WeatherForecast
            {
                Date = DateTime.Now.AddDays(i),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
            
            weathers.Add(forecast);
        }
    
        return weathers.ToArray();
    }

    [HttpGet(Name = "HelloWorld")]
    // GET WeatherForecast/GetHelloWorld
    public string GetHelloWorld()
    {
        return "Hello world";
    }
    
    // GET WeatherForecast/GetSum/10/20
    [HttpGet("{x}/{y}")]
    public int GetSum(int x, int y)
    {
        return x + y;
    }
    
    // GET WeatherForecast/GetHelloWorld?x=10&y=20
    [HttpGet]
    public int GetSumByParams([FromQuery] int x, [FromQuery] int y)
    {
        return x + y;
    }
}