using System.Collections.Generic;

public struct CommandSet
{
    List<IActorCommands> commands;

    public CommandSet(List<IActorCommands> commands)
    {
        this.commands = commands;
    }
    public void ExecuteSet()
    {
        List<IActorCommands> failedCommands = new List<IActorCommands>();
        for (int i = 0; i < commands.Count; i++)
        {
            commands[i].Evaluate();
            if (commands[i].Result == true)
                commands[i].ExecuteCommand();
            //Move failed commands to own list and discard
            else
            {
                failedCommands.Add(commands[i]);
                commands.RemoveAt(i);
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
                failedCommands[i].Evaluate();
                if (failedCommands[i].Result == true)
                {
                    failedCommands[i].ExecuteCommand();
                    isCommandSuccesful = true;
                    commands.Add(failedCommands[i]);
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
    public void UndoSet()
    {
        foreach(IActorCommands command in commands)
        {
            command.Undo();
        }
    }

    public bool IsSetProductive()
    {
        foreach(IActorCommands command in commands)
        {
            if (command.Result == true)
                return true;
        }
        return false;
    }

    public List<ActorObject> GetPushedActorObjects()
    {
        List<ActorObject> pushedObjects = new List<ActorObject>();
        foreach(ActorMoveCommand command in commands)
        {
            if(command != null)
            {
                if(command.PushedActor != null)
                    pushedObjects.Add(command.PushedActor);
            }
        }
        return pushedObjects;
    }
}