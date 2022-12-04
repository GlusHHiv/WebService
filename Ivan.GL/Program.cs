namespace AsyncExamples;

public static class Program
{
    public static async Task Main(string[] args)
    {

        for (int i = 0; i < 100; i++)
        {

            var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync("https://api.lorem.space/image/movie?w=2000&h=2000");

            var fileName = "imgs/" + i + ".png";

            FileStream fileStream = File.Create(fileName);
            fileStream.WriteAsync(bytes);
            fileStream.Close();
        }
        /*var client = new HttpClient();
        var bytes = await client.GetByteArrayAsync("https://api.lorem.space/image/movie?w=2000&h=2000");

        var fileName = "imgs/" + Guid.NewGuid() + ".png";

        var fileStream = File.(fileOpenName, FileMode.Create);
        await fileStream.WriteAsync(bytes);
        fileStream.Close();*/

        Console.WriteLine("Main ended");
    }

    public static double Sum(IEnumerable<int> data) => data.Sum(x => (double)x);
    public static async Task<double> SumAsync(IEnumerable<int> data)
    {
        await Task.Delay(2000);
        return data.Sum(x => (double)x);
    }

    public static void DoWork(int timeSleep, string endString)
    {
        Console.WriteLine("DoWork started");
        Thread.Sleep(timeSleep * 1000);
        Console.WriteLine(endString);
        Console.WriteLine("DoWork ended");
    }
}
