using System.Collections.Generic;

public class TurnCommands
{
    public List<IActorCommands> Commands;

    public TurnCommands()
    {
        Commands = new List<IActorCommands>();
    }

    public void ExecuteCommands()
    {
        foreach(IActorCommands command in Commands)
        {
            command.ExecuteCommand();
        }
    }

    public void UndoCommands()
    {
        foreach(IActorCommands command in Commands)
        {
            command.Undo();
        }
    }
}