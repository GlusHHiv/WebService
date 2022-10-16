namespace webService.WebApi;

public class Human
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public List<Hobby> Hobbies { get; set; }
}

public class Hobby
{
    public string Name { get; set; }
}

public enum Gender
{
    None,
    Man,
    Woman,
    Helicopter
}