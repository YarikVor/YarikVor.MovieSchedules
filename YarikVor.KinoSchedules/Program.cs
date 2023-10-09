using System.Text;

namespace YarikVor.KinoSchedules;

public class Program
{
    private static readonly ConsoleRunner ConsoleRunner = new();

    public static async Task Main()
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;

        await ConsoleRunner.InitAsync();
        await ConsoleRunner.RunAsync();

        await Task.Delay(-1);
    }
}