namespace SalvatalonMud;

internal class CommandResult
{
    public string Message { get; }
    public bool ShouldContinue { get; }
    public string? RoomMessage { get; }

    public CommandResult(string message, bool shouldContinue, string? roomMessage = null)
    {
        Message = message;
        ShouldContinue = shouldContinue;
        RoomMessage = roomMessage;
    }
}