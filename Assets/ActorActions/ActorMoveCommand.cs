using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ActorMoveCommand : IActorCommands
{
    private ActorObject actor;
    private MapManager mapManager;
    private ActorManager actorManager;
    private GameDirection direction;
    private ActorMoveCommand innerCommand;

    public bool Result { get; private set; }

    public ActorMoveCommand(ActorObject actor, GameDirection direction, MapManager mapManager, ActorManager actorManager)
    {
        this.actor = actor;
        this.direction = direction;
        this.mapManager = mapManager;
        this.actorManager = actorManager;
    }
    public void ExecuteCommand()
    {
        Result = true;
        Vector2Int newPosition = DirectionHelper.GetPositionInDirection(actor.GamePosition, direction);
        if (!GameUtilities.IsPositionInBounds(actorManager.ActorsGrid, newPosition))
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
                if (actorType.CanPush && otherActorType.CanBePushed)
                {
                    innerCommand = new ActorMoveCommand(otherActor, direction, mapManager, actorManager);
                    innerCommand.ExecuteCommand();
                    Result = innerCommand.Result;
                }
                else
                {
                    Result = false;
                }
            }
        }

        if (Result == false)
        {
            innerCommand = null;
            return;
        }
        actorManager.MoveActor(actor, DirectionHelper.GetDirectionVector(direction));


    }

    public void Undo()
    {
        if(Result == true)
        {
            GameDirection opposite = DirectionHelper.GetOppositeDirection(direction);
            actorManager.MoveActor(actor, DirectionHelper.GetDirectionVector(opposite));
            if (innerCommand != null)
            {
                innerCommand.Undo();
            }
        }
    }
}