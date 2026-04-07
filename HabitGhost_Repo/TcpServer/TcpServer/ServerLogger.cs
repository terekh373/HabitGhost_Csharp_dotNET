public static class ServerLogger
{
    private static readonly string logFilePath = "ServerActions.txt";
    private static readonly object lockObj = new object();

    public static void Log(string message)
    {
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string fullMessage = $"[{timeStamp}] {message}";

        Console.WriteLine(fullMessage);

        lock (lockObj)
        {
            File.AppendAllText(logFilePath, fullMessage + Environment.NewLine);
        }
    }
}
