/// <summary>
/// Implement game actions with this so its compatible with the command stack
/// </summary>
public interface IActorCommands
{
    bool Result { get; }
    void Evaluate();
    void ExecuteCommand();

    void Undo();
}