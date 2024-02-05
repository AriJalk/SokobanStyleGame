using UnityEngine.Events;

public class PlayableActorType : ActorType
{
    private IActorCommands nextStepCommand;
    public UnityEvent<IActorCommands> inputEvents { get; private set; }
    public PlayableActorType(string name) : base(name)
    {

    }

    private void SetNextStepCommand(IActorCommands command)
    {
        nextStepCommand = command;
    }

    public void SetInputEvents(UnityEvent<IActorCommands> inputEvents)
    {
        this.inputEvents = inputEvents;
        inputEvents.AddListener(SetNextStepCommand);
    }

    public void RemoveInputEvents()
    {
        if (inputEvents != null)
        {
            inputEvents.RemoveListener(SetNextStepCommand);
        }
    }

    public override void ExecuteNextStep()
    {
        nextStepCommand.ExecuteCommand();
    }

}