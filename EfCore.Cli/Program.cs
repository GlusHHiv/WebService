using EfCore.Cli;
using Microsoft.EntityFrameworkCore;

var anton = new User
{
    Name = "Anton"
};

var warAndPiec = new Book
{
    Name = "Война и мир",
    Author = anton
};

var cleanArchitecture = new Book
{
    Name = "Чистая архитектура",
    Author = anton
};

var richter = new User
{
    Name = "Джеффри"
};

var clrViaCsharp = new Book
{
    Name = "Clr via c#",
    Author = richter
};


Console.WriteLine(anton.Name);
Console.WriteLine(richter.Name);

using (var dbContext = new ApplicationDbContext())
{
    dbContext.Users.Add(anton);
    dbContext.Users.Add(richter);
    dbContext.Books.Add(warAndPiec);
    dbContext.Books.Add(cleanArchitecture);
    dbContext.Books.Add(clrViaCsharp);
    dbContext.SaveChanges(); // Сохранили в базе
}

using (var dbAnotherContext = new ApplicationDbContext())
{
    var users = dbAnotherContext.Users.Include(user => user.Books).ToList();
    foreach (var user in users)
    {
        Console.WriteLine($"FROM DB: \tID: {user.Id}\tName:{user.Name}");
        foreach (var book in user.Books)
        {
            Console.WriteLine($"\tFROM DB: \tID: {book.Id}\tName:{book.Name}");
        }
    }
}