using System.Collections.Generic;

public class TileObject : ActorObject
{
    public List<BorderStruct> Borders;

    public void Initialize()
    {
        Borders = new List<BorderStruct>();
    }
}