public class NewMoveCommand : IActorCommands
{
    public bool Result { get; private set; }

    public void ExecuteCommand()
    {
        throw new System.NotImplementedException();
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}