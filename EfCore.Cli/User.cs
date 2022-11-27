namespace EfCore.Cli;

public class User : IEquatable<User>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Book> Books { get; set; }

    public bool Equals(User? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        return Id;
    }
 
    public static bool operator ==(User? user1, User? user2)
    {
        return user1?.Equals(user2) ?? false;
    }

    public static bool operator !=(User? user1, User? user2)
    {
        return !(user1 == user2);
    }
}

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public Publisher Publisher { get; set; }
}

public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; }
}