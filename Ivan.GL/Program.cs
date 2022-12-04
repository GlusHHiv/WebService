using System.Collections;
using System.Diagnostics;

namespace AsyncExamples;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Последовательное скачивание
        // for (int i = 0; i < 100; i++)
        // {
        //     var client = new HttpClient();
        //     var bytes = await client.GetByteArrayAsync("https://api.lorem.space/image/movie?w=2000&h=2000");
        //
        //     var fileName = "imgs/" + i + ".png";
        //     
        //     FileStream fileStream = File.Create(fileName);
        //     await fileStream.WriteAsync(bytes);
        //     fileStream.Close();
        //     total = stopwatch.ElapsedMilliseconds;
        //     Console.WriteLine($"{i} download with {total}");
        // }
        
        // Парралельные способы
        Var1();
        Var2();
    }

    private static void Var2()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Console.WriteLine("Downloading starting");
        double total;
        var tasks = new List<Task>();
        for (int i = 0; i < 100; i++)
        {
            var fileName = "imgs2/" + i + ".png";
            tasks.Add(new Task(() => DownloadImage(fileName, stopwatch)));
            // tasks.Add(Task.Run(() => DownloadImage(fileName, stopwatch)));
        }

        foreach (var task in tasks)
        {
            task.Start();
        }

        Task.WaitAll(tasks.ToArray());
        stopwatch.Stop();
        total = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Downloading ended with {total}ms");
    }
    
    private static void Var1()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Console.WriteLine("Downloading starting");
        double total;
        var tasks = new Queue<Task>();
        var runningTasks = new List<Task>();
        for (int i = 0; i < 100; i++)
        {
            var fileName = "imgs2/" + i + ".png";
            tasks.Enqueue(new Task(() => DownloadImage(fileName, stopwatch)));
        }

        while (tasks.Count > 0)
        {
            if (runningTasks.Count > Environment.ProcessorCount)
            {
                Thread.Sleep(10);
                for (var i = 0; i < runningTasks.Count; i++)
                {
                    var runningTask = runningTasks[i];
                    if (runningTask.IsCompleted)
                    {
                        runningTasks.Remove(runningTask);
                    }
                }

                continue;
            }

            var task = tasks.Dequeue();
            task.Start();
            runningTasks.Add(task);
        }

        stopwatch.Stop();
        total = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Downloading ended with {total}ms");
    }

    public static async Task DownloadImageAsync(string fileName, Stopwatch stopwatch)
    {
        var client = new HttpClient();
        var bytes = await client.GetByteArrayAsync("https://api.lorem.space/image/movie?w=2000&h=2000");
        FileStream fileStream = File.Create(fileName);
        await fileStream.WriteAsync(bytes);
        fileStream.Close();
        Console.WriteLine($"{fileName} download with {stopwatch.ElapsedMilliseconds}");
    }
    
    public static void DownloadImage(string fileName, Stopwatch stopwatch)
    {
        var client = new HttpClient();
        var bytes = client.GetByteArrayAsync("https://api.lorem.space/image/movie?w=2000&h=2000").GetAwaiter().GetResult();
        FileStream fileStream = File.Create(fileName);
        fileStream.Write(bytes);
        fileStream.Close();
        Console.WriteLine($"{fileName} download with {stopwatch.ElapsedMilliseconds}");
    }
}
