using System.Collections.Generic;

/// <summary>
/// Represents tiles in the scene
/// </summary>
public class TileObject : ActorObject
{
    public List<GameBorderStruct> Borders;

    public void Initialize()
    {
        Borders = new List<GameBorderStruct>();
    }
}