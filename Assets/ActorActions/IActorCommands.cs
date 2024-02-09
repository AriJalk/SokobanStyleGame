public interface IActorCommands
{
    bool Result { get; }
    void Evaluate();
    void ExecuteCommand();

    void Undo();
}