using UnityEngine;

public static class DirectionHelper
{
    public const GameDirection Left = GameDirection.Left;
    public const GameDirection Right = GameDirection.Right;
    public const GameDirection Up = GameDirection.Up;
    public const GameDirection Down = GameDirection.Down;
    public const GameDirection Neutral = GameDirection.Neutral;

    public static GameDirection GetOppositeDirection(GameDirection direction)
    {
        switch (direction)
        {
            case Left:
                return Right;
            case Right:
                return Left;
            case Up:
                return Down;
            case Down:
                return Up;
            case Neutral:
            default:
                return Neutral;
        }
    }

    public static Vector2Int GetDirectionVector(GameDirection direction)
    {
        switch (direction)
        {
            case Left:
                return Vector2Int.left;
            case Right:
                return Vector2Int.right;
            case Up:
                return Vector2Int.up;
            case Down:
                return Vector2Int.down;
            case Neutral:
            default:
                return Vector2Int.zero;
        }
    }

    public static Vector2Int GetPositionInDirection(Vector2Int origin, GameDirection direction)
    {
        Vector2Int position;
        switch(direction)
        {
            case Left:
                position = origin + Vector2Int.left;
                break;
            case Right:
                position = origin + Vector2Int.right;
                break;
            case Up:
                position = origin + Vector2Int.up;
                break;
            case Down:
                position = origin + Vector2Int.down;
                break;
            case Neutral:
            default:
                position = origin;
                break;
        }
        return position;
    }
}