using UnityEngine;

public class ActorRotateCommand : IActorCommands
{
    private ActorObject actor;
    private GameRotation rotation;
    public ActorRotateCommand(ActorObject actor, GameRotation rotation)
    {
        this.actor = actor;
        this.rotation = rotation;
    }



    public void ExecuteCommand()
    {
        actor.SetGameRotation(rotation);

    }
}