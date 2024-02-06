using UnityEngine.Events;

public class PlayableActorType : ActorType
{
    const string NAME = "Playable Actor";
    private IActorCommands nextStepCommand;
    public UnityEvent<IActorCommands> inputEvents { get; private set; }
    public PlayableActorType(UnityEvent<IActorCommands> inputEvents) : base(NAME)
    {
        ResourceName = "Player";
        this.inputEvents = inputEvents;
        inputEvents.AddListener(SetNextStepCommand);
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