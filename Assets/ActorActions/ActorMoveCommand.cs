using System.Numerics;
using UnityEngine;

public class ActorMoveCommand : IActorCommands
{
    private ActorObject actor;
    private Vector2Int newPosition;

    private TileObject[,] map;
    private bool canExecute;
    public ActorMoveCommand(ActorObject actor, GameDirection direction, short steps, TileObject[,] map)

    {
        canExecute = false;
        this.actor = actor;
        this.map = map;
        newPosition = actor.GamePosition;
        switch (direction)
        {
            case GameDirection.Left:
                newPosition += Vector2Int.left;
                break;
            case GameDirection.Right:
                newPosition += Vector2Int.right;
                break;
            case GameDirection.Up:
                newPosition += Vector2Int.up;
                break;
            case GameDirection.Down:
                newPosition += Vector2Int.down;
                break;
            case GameDirection.UpLeft:
                newPosition += Vector2Int.up + Vector2Int.left;
                break;
            case GameDirection.UpRight:
                newPosition += Vector2Int.up + Vector2Int.right;
                break;
            case GameDirection.DownLeft:
                newPosition += Vector2Int.down + Vector2Int.left;
                break;
            case GameDirection.DownRight:
                newPosition += Vector2Int.down + Vector2Int.right;
                break;
            case GameDirection.Wait:
                break;
            default:
                break;
        }

        if (newPosition.x >= 0 && newPosition.x <= map.GetLength(0) - 1 &&
            newPosition.y >= 0 && newPosition.y <= map.GetLength(1) - 1 &&
            map[newPosition.x, newPosition.y].ObjectSlot == null)

        {
            TileObject tile = map[newPosition.x, newPosition.y];
            canExecute = true;
        }
    }



    public void ExecuteCommand()
    {
        if (canExecute == true)
        {
            actor.SetGamePosition(newPosition);

        }
    }
}