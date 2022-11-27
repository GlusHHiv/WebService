using EfCore.Cli;
using Microsoft.EntityFrameworkCore;

void PrintUsersFromDb()
{
    Console.WriteLine(new string('-', 20));
    using (var dbAnotherContext = new ApplicationDbContext())
    {
        var users = dbAnotherContext.Users
            .Include(user => user.Books).ThenInclude(book => book.Publisher)
            .AsNoTracking().ToList();
        foreach (var user in users)
        {
            Console.WriteLine($"FROM DB: \tID: {user.Id}\tName:{user.Name}");
            foreach (var book in user.Books)
            {
                Console.WriteLine($"\tFROM DB: \tID: {book.Id}\tName:{book.Name}\tPublisher:{book.Publisher.Name}");
            }
        }
    }
}

var anton = new User
{
    Id = 1,
    Name = "Anton"
};

var anton2 = new User
{
    Id = 1,
    Name = "Anton"
};

var anton3 = anton;

Console.WriteLine(anton == anton2);
Console.WriteLine(anton == anton3);
Console.WriteLine(anton.GetHashCode());
Console.WriteLine(anton2.GetHashCode());
Console.WriteLine(anton3.GetHashCode());


Console.WriteLine(anton == anton2);

var spb = new Publisher()
{
    Name = "СПБ"
};

var msk = new Publisher()
{
    Name = "МСК"
};

var warAndPiec = new Book
{
    Name = "Война и мир",
    Author = anton,
    Publisher = spb
};

var cleanArchitecture = new Book
{
    Name = "Чистая архитектура",
    Author = anton,
    Publisher = spb

};

var richter = new User
{
    Name = "Джеффри"
};

var clrViaCsharp = new Book
{
    Name = "Clr via c#",
    Author = richter,
    Publisher = msk
};


Console.WriteLine(anton.Name);
Console.WriteLine(richter.Name);

using (var dbContext = new ApplicationDbContext(true))
{
    dbContext.Users.Add(anton);
    dbContext.Users.Add(richter);
    dbContext.Books.Add(warAndPiec);
    dbContext.Books.Add(cleanArchitecture);
    dbContext.Books.Add(clrViaCsharp);
    dbContext.SaveChanges(); // Сохранили в базе
}

PrintUsersFromDb();


// Обновление модели в одном контесте
// Мы обновляем модель в том же контексте, в котором мы ее получили 
using (var dbcontext = new ApplicationDbContext())
{
    var findUser = dbcontext.Users.FirstOrDefault(user => user.Name == "Anton");
    if (findUser == null) throw new Exception();
    findUser.Name = "Sasha";
    await dbcontext.SaveChangesAsync();
}

PrintUsersFromDb();

// Обновление в разных котекстах
// Получаем модель в первом контексте, а во втором ее обновляем и сохраняем
User findUser2;
using (var dbcontext = new ApplicationDbContext())
{
    findUser2 = dbcontext.Users.FirstOrDefault(user => user.Name == "Джеффри");
    if (findUser2 == null) throw new Exception();
}

using (var updateContext = new ApplicationDbContext())
{
    findUser2.Name = "Джеффри Рихтер";
    updateContext.Entry(findUser2).State = EntityState.Modified;
    updateContext.SaveChanges();
}

PrintUsersFromDb();
Console.WriteLine(new string('-', 20));
// Подгрузка навигационных полей
using (var loadBookContext = new ApplicationDbContext())
{
    var findUser2Books = loadBookContext.Books.Where(book => book.Author.Id == findUser2.Id).ToList();
    foreach (var book in findUser2Books)
    {
        Console.WriteLine(book.Name);
    }
    Console.WriteLine(new string('-', 20));

    loadBookContext.Attach(findUser2).Collection(user => user.Books).Load();
    foreach (var book in findUser2.Books)
    {
        Console.WriteLine(book.Name);
    }
    Console.WriteLine(new string('-', 20));
}

Console.WriteLine(new string('-', 20));
// Подгрузка навигационных полей
using (var loadBookContext = new ApplicationDbContext())
{
    var books = loadBookContext.Books.AsNoTracking().ToList();
    // loadBookContext.Attach(books).Reference(books => books.Select(book => book.Author)).Load();
    foreach (var book in books)
    {
        loadBookContext.Attach(book).Reference(book => book.Author).Load();
        Console.WriteLine(book.Author.Name + "\t" +  book.Name);
    }
    Console.WriteLine(new string('-', 20));
}

using (var addBookContext = new ApplicationDbContext())
{
    var user = addBookContext.Users
        .AsNoTracking()
        .Include(user => user.Books)
        .FirstOrDefault(user => user.Id == 2);
    
    var book = new Book
    {
        Name = "Чистый код",
        Publisher = msk
    };
    addBookContext.Attach(user);
    user.Books.Add(book);
    addBookContext.SaveChanges();
}

PrintUsersFromDb();

using (var addBookContext = new ApplicationDbContext())
{
    var user = addBookContext.Users
        .AsNoTracking()
        .Include(user => user.Books)
        .FirstOrDefault(user => user.Id == 2);
    
    var book = new Book
    {
        Name = "Чистый код",
        Publisher = msk
    };
    user.Books.Add(book);
    addBookContext.Attach(user);
    addBookContext.SaveChanges();
}

PrintUsersFromDb();

using (var addBookContext = new ApplicationDbContext())
{
    var user = addBookContext.Users
        .AsNoTracking()
        .Include(user => user.Books)
        .FirstOrDefault(user => user.Id == 2);
    
    var book = new Book
    {
        Name = "Чистый код",
        Publisher = msk
    };
    // addBookContext.Attach(user);
    // как средствами ef core подзагрузить у всех не отслеживаемых user.books навигационное поле publisher
    addBookContext.Attach(user).Collection(u => u.Books).Query().Include(b => b.Publisher).Load();
    user.Books.Add(book);
    addBookContext.SaveChanges();

    foreach (var userBook in user.Books)
    {
        Console.WriteLine(userBook.Publisher.Name);
    }
}

PrintUsersFromDb();