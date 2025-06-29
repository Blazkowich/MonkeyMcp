namespace MonkeyMcp.Core.Exceptions;

public sealed class MonkeyNotFoundException : Exception
{
    public string MonkeyName { get; }

    public MonkeyNotFoundException(string monkeyName) 
        : base($"Monkey with name '{monkeyName}' was not found.")
    {
        MonkeyName = monkeyName;
    }

    public MonkeyNotFoundException(string monkeyName, Exception innerException) 
        : base($"Monkey with name '{monkeyName}' was not found.", innerException)
    {
        MonkeyName = monkeyName;
    }
}