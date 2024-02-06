using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ActorMoveCommand : IActorCommands
{
    private ActorObject actor;
    private MapManager mapManager;
    private ActorManager actorManager;
    private GameDirection direction;

    public ActorMoveCommand(ActorObject actor, GameDirection direction, MapManager mapManager, ActorManager actorManager)
    {
        this.actor = actor;
        this.direction = direction;
        this.mapManager = mapManager;
        this.actorManager = actorManager;
    }
    public void ExecuteCommand()
    {
        bool result = true;
        Vector2Int newPosition = DirectionHelper.GetPositionInDirection(actor.GamePosition, direction);
        if (!GameUtilities.IsPositionInBounds(actorManager.ActorsGrid, newPosition))
            result = false;
        TileObject tileA = mapManager.GetTile(actor.GamePosition);
        TileObject tileB = mapManager.GetTile(newPosition);
        BorderStruct borderStruct = new BorderStruct(tileA, tileB);
        if (mapManager.Borders.ContainsKey(borderStruct))
            result = false;
        if (actorManager.GetActor(newPosition) != null)
            result = false;

        if(result == false)
        {
            direction = GameDirection.Neutral;
            return;
        }
        actorManager.MoveActor(actor, DirectionHelper.GetDirectionVector(direction));


    }

    public void Undo()
    {
        GameDirection opposite = DirectionHelper.GetOppositeDirection(direction);
        actorManager.MoveActor(actor, DirectionHelper.GetDirectionVector(opposite));
    }
}