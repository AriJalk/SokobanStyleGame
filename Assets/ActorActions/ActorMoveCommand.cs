using System.Numerics;
using UnityEngine;

public class ActorMoveCommand : IActorCommands
{
    private ActorObject actor;
    private Vector2Int originPosition;
    private Vector2Int targetPosition;

    private MapManager map;
    public bool CanExecute { get; private set; }

    public ActorMoveCommand(Vector2Int originPosition, Vector2Int direction, TileObject[,] tileGrid, ActorObject[,] actorGrid)
    {
        this.originPosition = originPosition;
        targetPosition = originPosition + direction;

        if (GameUtilities.IsPositionInBounds(map.MapGrid, targetPosition) == true)
        {
            if (actorGrid[originPosition.x,originPosition.y] != null)
            {
                // Empty
                if (actorGrid[targetPosition.x,targetPosition.y] == null)
                {
                    CanExecute = true;
                }
                // Push
                else
                {

                }
            }
        }
    }



    public void ExecuteCommand()
    {
        if (CanExecute == true)
        {

        }
    }

    public void Undo()
    {
        actor.SetGamePosition(originPosition);
    }
}