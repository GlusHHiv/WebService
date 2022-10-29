namespace EfCore.Cli;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Book> Books { get; set; }
}

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public int AuthorId { get; set; }
    public User Author { get; set; }
}