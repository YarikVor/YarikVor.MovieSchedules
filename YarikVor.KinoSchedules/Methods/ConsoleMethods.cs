namespace YarikVor.KinoSchedules;

public static class ConsoleMethods
{
    public static int InputInt32(string text)
    {
        int result;
        do
        {
            Console.Write(text);
        } while (!int.TryParse(Console.ReadLine(), out result));

        return result;
    }
    
    public static void Pause()
    {
        Console.WriteLine("Press any key for continued...");
        Console.ReadKey();
    }
}