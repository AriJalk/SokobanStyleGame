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
        for (int i = 0; i < Commands.Count; i++)
        {
            Commands[i].ExecuteCommand();
            //Discard failed commands
            if (Commands[i].Result == false)
            {
                Commands.RemoveAt(i);
            }
        }
    }

    public void UndoCommands()
    {
        foreach (IActorCommands command in Commands)
        {
            command.Undo();
        }
    }
}