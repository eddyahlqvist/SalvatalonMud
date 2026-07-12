namespace SalvatalonMud;

internal class CommandResult
{
    public string Message { get; }
    public bool ShouldContinue { get; }

    public CommandResult(string message, bool shouldContinue)
    {
        Message = message;
        ShouldContinue = shouldContinue;
    }
}