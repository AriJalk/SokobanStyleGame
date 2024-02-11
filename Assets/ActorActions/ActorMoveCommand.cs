using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

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
        Vector2Int newPosition = DirectionHelper.GetPositionInDirection(actor.GamePosition, direction);
        if (!GameUtilities.IsPositionInBounds(actorManager.ActorsGrid, newPosition) || mapManager.GetTile(newPosition) == null)
            Result = false;
        if (Result == true)
        {
            ActorObject tileA = mapManager.GetTile(actor.GamePosition);
            ActorObject tileB = mapManager.GetTile(newPosition);
            BorderStruct borderStruct = new BorderStruct(tileA, tileB);
            if (mapManager.Borders.ContainsKey(borderStruct))
                Result = false;
            ActorObject otherActor = actorManager.GetActor(newPosition);
            if (otherActor != null && otherActor != actor && Result == true)
            {
                EntityActorType actorType = actor.ActorType as EntityActorType;
                EntityActorType otherActorType = otherActor.ActorType as EntityActorType;
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

            actorManager.MoveActor(actor, DirectionHelper.GetDirectionVector(direction));

        }
    }

    public void Undo()
    {
        if (Result == true)
        {
            GameDirection opposite = DirectionHelper.GetOppositeDirection(direction);
            actorManager.MoveActor(actor, DirectionHelper.GetDirectionVector(opposite));
            if (innerCommand != null && innerCommand.Result == true)
                innerCommand.Undo();
        }
    }
}