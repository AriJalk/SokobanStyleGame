using System;
using System.Collections.Generic;
using UnityEngine;

public struct LevelStruct
{
    public char[,] TileGrid {  get; private set; }
    public char[,] EntityGrid {get; private set;}
    public List<Tuple<Vector2Int, Vector2Int>> Borders { get; private set;}

    public LevelStruct(char[,] tileGrid, char[,] entityGrid, List<Tuple<Vector2Int, Vector2Int>> borders)
    {
        this.TileGrid = tileGrid;
        this.EntityGrid = entityGrid;
        this.Borders = borders;
        
    }
}