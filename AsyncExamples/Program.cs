namespace AsyncExamples;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Console.WriteLine("Main started");
        // var task1 = new Task(() => DoWork(5, "Task1"));
        // var task2 = new Task(() => DoWork(3, "Task2"));
        // var task3 = new Task(() => DoWork(10, "Task3"));
        // var task4 = Task.Run(() => DoWork(10, "Task4"));
        // task1.Start();
        // task2.Start();
        // task3.Start();
        // Console.WriteLine("Waiting Task1");
        // Console.WriteLine("Waiting Task2");
        // Console.WriteLine("Waiting Task3");
        // Console.WriteLine("Waiting Task4");
        // Task.WaitAll(new[] { task1, task2, task3, task4 });
        // task1.Wait();
        // task2.Wait();
        // task3.Wait();
        // task4.Wait();
        //
        // var data = Enumerable.Range(0, 1000 * 1000 * 1000).ToList();
        // var taskCount = 8;
        // var bucketSize = data.Count / taskCount;
        // var tasks = new List<Task<double>>();
        //
        // for (var i = 0; i < taskCount; i++)
        // {
        //     var bucket = data.Skip(i*bucketSize).Take(bucketSize);
        //     var task = new Task<double>(() => Sum(bucket));
        //     tasks.Add(task);
        // }
        //
        // Console.WriteLine("Tasks created");
        // foreach (var task in tasks)
        // {
        //     task.Start();
        // }
        // Console.WriteLine("Tasks started");
        //
        // Console.WriteLine("Tasks waiting");
        // Task.WaitAll(tasks.ToArray());
        //
        // Console.WriteLine("Tasks completed");
        // var sum = 0d;
        // foreach (var task in tasks)
        // {
        //     sum += task.Result;
        // }
        //
        // Console.WriteLine(sum);
        // Console.WriteLine("Main ended");
        
        // var data = Enumerable.Range(0, 1000 * 1000 * 1000).ToList();
        // var taskCount = 8;
        // var bucketSize = data.Count / taskCount;
        // var tasks = new List<Task<double>>();
        //
        // for (var i = 0; i < taskCount; i++)
        // {
        //     var bucket = data.Skip(i*bucketSize).Take(bucketSize);
        //     // var task = new Task<double>(() => Sum(bucket));
        //     var task = SumAsync(bucket);
        //     // var sumBucket = await SumAsync(bucket);
        //     // var sumBucket = SumAsync(bucket).GetAwaiter().GetResult();
        //     tasks.Add(task);
        // }
        //
        // Console.WriteLine("Tasks created");
        // // foreach (var task in tasks)
        // // {
        // //     task.Start();
        // // }
        // // Console.WriteLine("Tasks started");
        //
        // Console.WriteLine("Tasks waiting");
        // Task.WaitAll(tasks.ToArray());
        //
        // Console.WriteLine("Tasks completed");
        // var sum = 0d;
        // foreach (var task in tasks)
        // {
        //     sum += task.Result;
        // }
        //
        // Console.WriteLine(sum);
        // Console.WriteLine("Main ended");
        
        var client = new HttpClient();
        var bytes = await client.GetByteArrayAsync("https://api.lorem.space/image/movie?w=2000&h=2000");
        
        var fileName = "imgs/" + Guid.NewGuid() + ".png";
        
        var fileStream = File.Open(fileName, FileMode.Create);
        await fileStream.WriteAsync(bytes);
        fileStream.Close();

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