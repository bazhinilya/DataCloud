Console.WriteLine(DateTime.UtcNow + ": " + "Start Data Processor.");
Console.WriteLine(DateTime.UtcNow + ": " + "Starting Listen folder.");
Console.WriteLine("Press <Escape> to exit!");
DataProcessor.FolderListener.FolderListener _ = new();

bool isExit = false;
while (!isExit)
{
    isExit = Console.ReadKey().Key == ConsoleKey.Escape;
}

Console.WriteLine(DateTime.UtcNow + ": " + "Stop Data Processor.");