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
        TileObject tileA = mapManager.GetTile(actor.GamePosition);
        TileObject tileB = mapManager.GetTile(newPosition);
        BorderStruct borderStruct = new BorderStruct(tileA, tileB);
        if (mapManager.Borders.ContainsKey(borderStruct))
            Result = false;
        ActorObject otherActor = actorManager.GetActor(newPosition);
        if (otherActor != null)
        {
            if (actor.ActorType.CanPush)
            {
                innerCommand = new ActorMoveCommand(otherActor, direction, mapManager, actorManager);
                innerCommand.ExecuteCommand();
                Result &= innerCommand.Result;
            }
            else
            {
                Result = false;
            }
        }

        if (Result == false)
        {
            direction = GameDirection.Neutral;
            innerCommand = null;
            return;
        }
        actorManager.MoveActor(actor, DirectionHelper.GetDirectionVector(direction));


    }

    public void Undo()
    {
        GameDirection opposite = DirectionHelper.GetOppositeDirection(direction);
        actorManager.MoveActor(actor, DirectionHelper.GetDirectionVector(opposite));
        if (innerCommand != null)
        {
            innerCommand.Undo();
        }
    }
}