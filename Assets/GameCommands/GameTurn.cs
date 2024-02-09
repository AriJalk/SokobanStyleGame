using System.Collections.Generic;

public class GameTurn
{
    public List<CommandSet> CommandSets { get; set; }

    public GameTurn()
    {
        CommandSets = new List<CommandSet>();
    }
    public void ExecuteSet(int index)
    {
        if (index < CommandSets.Count)
        {
            CommandSets[index].ExecuteSet();
        }
    }

    public void ExecuteAllSets()
    {
       foreach(CommandSet commandSet in CommandSets)
        {
            commandSet.ExecuteSet();
        }
    }

    public void UndoSet(int index)
    {
        if (index < CommandSets.Count && CommandSets[index].IsSetProductive() == true)
        {
            CommandSets[index].UndoSet();
        }
    }

    public void UndoAllSets()
    {
        foreach(CommandSet commandSet in CommandSets)
        {
            if(commandSet.IsSetProductive() == true)
                commandSet.UndoSet();
        }
    }

    public bool IsTurnProductive()
    {
        foreach(CommandSet commandSet in CommandSets)
        {
            if (commandSet.IsSetProductive() == true)
                return true;
        }
        return false;
    }
}