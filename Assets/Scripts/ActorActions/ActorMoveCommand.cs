using UnityEngine;

/// <summary>
/// Used to move an actor on the grid in desired direction
/// </summary>
public class ActorMoveCommand : IActorCommands
{
    private ActorObject actor;
    private MapManager mapManager;
    private ActorManager actorManager;
    private GameDirection direction;
    private ActorMoveCommand innerCommand;


    public bool Result { get; private set; }
    public ActorObject PushedActor { get; private set; }

    public ActorMoveCommand(ActorObject actor, GameDirection direction, MapManager mapManager, ActorManager actorManager)
    {
        this.actor = actor;
        this.direction = direction;
        this.mapManager = mapManager;
        this.actorManager = actorManager;
    }
    public void Evaluate()
    {
        Result = true;
        // Check if destination exists
        Vector2Int newPosition = GameUtilities.GetPositionInDirection(actor.GamePosition, direction);
        if (!GameUtilities.IsPositionInBounds(actorManager.ActorsGrid, newPosition) || mapManager.GetTile(newPosition) == null)
            Result = false;
        if (Result == true)
        {
            // Check if there's a border between source and destination
            ActorObject tileA = mapManager.GetTile(actor.GamePosition);
            ActorObject tileB = mapManager.GetTile(newPosition);
            GameBorderStruct borderStruct = new GameBorderStruct(tileA, tileB);
            if (mapManager.Borders.ContainsKey(borderStruct))
                Result = false;
            // Check if another actor is in the destination
            ActorObject otherActor = actorManager.GetActor(newPosition);
            if (otherActor != null && otherActor != actor && Result == true)
            {
                // Check if the actor can cascade a push further
                EntityActorTypeBase actorType = actor.ActorType as EntityActorTypeBase;
                EntityActorTypeBase otherActorType = otherActor.ActorType as EntityActorTypeBase;
                Result = actorType.CanPush & otherActorType.CanBePushed;
                if (Result == true)
                {
                    innerCommand = new ActorMoveCommand(otherActor, direction, mapManager, actorManager);
                    innerCommand.Evaluate();
                    if (innerCommand.Result == false)
                    {
                        Result = false;
                        innerCommand = null;
                    }
                    else
                    {
                        PushedActor = otherActor;
                    }
                }
            }
        }
    }
    public void ExecuteCommand()
    {
        if (Result == true)
        {
            if (innerCommand != null && innerCommand.Result == true)
                innerCommand.ExecuteCommand();

            actorManager.MoveActor(actor, GameUtilities.GetDirectionVector(direction));

        }
    }

    public void Undo()
    {
        if (Result == true)
        {
            GameDirection opposite = GameUtilities.GetOppositeDirection(direction);
            actorManager.MoveActor(actor, GameUtilities.GetDirectionVector(opposite));
            if (innerCommand != null && innerCommand.Result == true)
                innerCommand.Undo();
        }
    }
}