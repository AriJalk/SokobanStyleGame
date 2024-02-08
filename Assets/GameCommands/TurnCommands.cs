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
        List<IActorCommands> failedCommands = new List<IActorCommands>();
        for (int i = 0; i < Commands.Count; i++)
        {
            Commands[i].ExecuteCommand();
            //Move failed commands to own list and discard
            if (Commands[i].Result == false)
            {
                failedCommands.Add(Commands[i]);
                Commands.RemoveAt(i);
                i--;
            }
        }
        bool isCommandSuccesful = true;
        // Run until no failed commands exist
        while (isCommandSuccesful || failedCommands.Count > 0)
        {
            isCommandSuccesful = false;
            // Try to execute again failed commands
            for (int i = 0; i < failedCommands.Count; i++)
            {
                failedCommands[i].ExecuteCommand();
                if (failedCommands[i].Result == true) 
                {
                    isCommandSuccesful = true;
                    Commands.Add(failedCommands[i]);
                    failedCommands.RemoveAt(i);
                    // Return succesfull command to list
                    i--;
                }
            }
            // If no command can be executed clear
            if (!isCommandSuccesful)
            {
                failedCommands.Clear();
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