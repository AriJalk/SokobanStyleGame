using UnityEngine;

public class MapManager
{
    public TileObject[,] MapGrid { get; private set; }
    public short SizeX { get; private set; }
    public short SizeY { get; private set; }

    public void SetMap(short xLength, short yLength)
    {
        SizeX = xLength;
        SizeY = yLength;
        MapGrid = new TileObject[SizeX, SizeY];
    }

}