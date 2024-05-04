using System.Diagnostics;

namespace Otus_Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        string folderPath = "C:\\Users\\dartw\\Documents";
        
        string filePath1 = "C:\\Users\\dartw\\Documents\\file_1.txt";
        string filePath2 = "C:\\Users\\dartw\\Documents\\file_2.txt";
        string filePath3 = "C:\\Users\\dartw\\Documents\\file_3.txt";

        string[] filePaths =
        [
            filePath1,
            filePath2,
            filePath3
        ];

        Task<int>[] tasks = new Task<int>[filePaths.Length];

        Stopwatch timer = new Stopwatch();
        
        timer.Start();

        for (var i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i];
            tasks[i] = Task.Run(() => CountSpacesInFileAsync(filePath));
        }

        int[] results = await Task.WhenAll(tasks);
        
        for (int i = 0; i < results.Length; i++)
        {
            Console.WriteLine($"Количество пробелов в файле '{filePaths[i]}': {results[i]}");
        }
        
        timer.Stop();

        Console.WriteLine($"Время выполнения {timer.ElapsedMilliseconds} мс");
        
        timer.Reset();
        Console.WriteLine();
        
        timer.Start();

        int counts = await CountSpacesInFolderAsync(folderPath);
        
        timer.Stop();
        
        Console.WriteLine($"Количество пробелов в папке '{folderPath}': {counts}");
        Console.WriteLine($"Время выполнения {timer.ElapsedMilliseconds} мс");
    }

    private static async Task<int> CountSpacesInFileAsync(string filePath)
    {
        string content = await File.ReadAllTextAsync(filePath);
        
        int count = 0;

        foreach (var c in content)
        {
            if (c == ' ')
            {
                count++;
            }
        }

        return count;
    }

    private static async Task<int> CountSpacesInFolderAsync(string folderPath)
    {
        int counts = 0;
        string[] filePaths = Directory.GetFiles(folderPath);

        Task<int>[] tasks = new Task<int>[filePaths.Length];
        
        for (var i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i];
            tasks[i] = Task.Run(() => CountSpacesInFileAsync(filePath));
        }

        int[] results = await Task.WhenAll(tasks);

        foreach (var t in results)
        {
            counts += t;
        }
        
        return counts;
    }
}