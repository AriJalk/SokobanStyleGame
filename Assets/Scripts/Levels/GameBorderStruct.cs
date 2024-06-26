﻿using UnityEngine;

public struct GameBorderStruct
{
    public ActorObject tileA;
    public ActorObject tileB;

    public GameBorderStruct(ActorObject tileA, ActorObject tileB)
    {
        this.tileA = tileA;
        this.tileB = tileB;
    }
    public bool IsBorderValid()
    {
        Vector2Int difference = tileA.GamePosition - tileB.GamePosition;
        if ((difference.x > 1 && difference.y > 1) || tileA == tileB)
        {
            return false;
        }
        return true;
    }

    public override bool Equals(object obj)
    {
        if (obj is GameBorderStruct other)
        {
            if ((tileA == other.tileA && tileB == other.tileB) ||
                (tileA == other.tileB && tileB == other.tileA))
                return true;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

