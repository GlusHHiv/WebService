namespace webApiMessenger.Shared.DTOs;

public class UserWithoutFriendsDTO
{
    public int Id { get; set; }
    public string Nick { get; set; }
    public int Age { get; set; }

    public override bool Equals(object? obj)
    {
        var other = obj as UserWithoutFriendsDTO;

        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Id == other.Id) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}