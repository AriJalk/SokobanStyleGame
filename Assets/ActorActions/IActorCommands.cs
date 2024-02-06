public interface IActorCommands
{
    bool Result { get; }
    void ExecuteCommand();

    void Undo();
}