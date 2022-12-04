namespace SergushkoVN;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var client = new HttpClient();

        for (var i = 0; i < 100; i++)
        {
            var bytes = await client.GetByteArrayAsync("https://api.lorem.space/image/movie?w=2000&h=2000");
            var fileName = "imgs/" + Guid.NewGuid() + ".png";
            var fileStream = File.Open(fileName, FileMode.Create);
            await fileStream.WriteAsync(bytes);
            fileStream.Close();
            Console.WriteLine("Main ended");
        }
    }
}