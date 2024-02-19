using System.Collections.Generic;

public class TileObject : ActorObject
{
    public List<GameBorderStruct> Borders;

    public void Initialize()
    {
        Borders = new List<GameBorderStruct>();
    }
}