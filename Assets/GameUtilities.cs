using System;
using UnityEngine;

public static class GameUtilities
{
    public static bool IsPositionInBounds (object[,] grid, Vector2Int position)
    {
        bool result = false;
        if (position.x >= 0 && position.x < grid.GetLength(0) &&
            position.y >= 0 && position.y < grid.GetLength(1))
        {
            result = true;
        }
        return result;
    }

    public static void SetParentAndResetPosition(Transform child, Transform parent)
    {
        child.SetParent(parent);
        child.localPosition = Vector3.zero;
        child.localScale = Vector3.one;
    }
}