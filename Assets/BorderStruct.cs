using System;
using UnityEngine;

public struct BorderStruct
{
    public ActorObject TileA;
    public ActorObject TileB;

    public BorderStruct(ActorObject tileA, ActorObject tileB)
    {
        this.TileA = tileA;
        this.TileB = tileB;
    }
    public bool IsBorderValid()
    {
        Vector2Int difference = TileA.GamePosition - TileB.GamePosition;
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

