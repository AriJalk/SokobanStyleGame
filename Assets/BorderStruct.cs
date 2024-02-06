using System;
using UnityEngine;

public struct BorderStruct
{
    public TileObject TileA;
    public TileObject TileB;

    public BorderStruct(TileObject tileA, TileObject tileB)
    {
        this.TileA = tileA;
        this.TileB = tileB;
    }
    public bool IsBorderValid()
    {
        Vector2Int difference = TileA.GridPosition - TileB.GridPosition;
        if ((difference.x > 1 && difference.y > 1) || TileA == TileB)
        {
            return false;
        }
        return true;
    }

    public override bool Equals(object obj)
    {
        if (obj is BorderStruct other)
        {
            if ((TileA == other.TileA && TileB == other.TileB) ||
                (TileA == other.TileB && TileB == other.TileA))
                return true;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

