using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelStruct
{
    [SerializeField]
    public string TileGridSerialized;
    [SerializeField]
    public string EntityGridSerialized;
    [SerializeField]
    public int GridSize;
    public char[,] TileGrid { get; private set; }
    public char[,] EntityGrid { get; private set; }


    public List<Tuple<Vector2Int, Vector2Int>> Borders { get; private set; }

    public LevelStruct(char[,] tileGrid, char[,] entityGrid, List<Tuple<Vector2Int, Vector2Int>> borders)
    {
        TileGrid = tileGrid;
        EntityGrid = entityGrid;
        Borders = borders;
        TileGridSerialized = string.Empty;
        EntityGridSerialized = string.Empty;
        GridSize = tileGrid.GetLength(0);
    }



    public void SerializeFields()
    {
        TileGridSerialized = string.Empty;
        EntityGridSerialized = string.Empty;
        for (int i = 0; i < TileGrid.GetLength(0); i++)
        {
            for (int j = 0; j < TileGrid.GetLength(1); j++)
            {
                if (TileGrid[i, j] == '\0')
                    TileGridSerialized += "x";
                else
                    TileGridSerialized += TileGrid[i, j];

                if (EntityGrid[i, j] == '\0')
                    EntityGridSerialized += 'x';
                else
                    EntityGridSerialized += EntityGrid[i, j];
            }
        }
    }
    public void DeserializeFields()
    {
        GridSize = (int)Mathf.Sqrt(TileGridSerialized.Length);
        TileGrid = new char[GridSize, GridSize];
        EntityGrid = new char[GridSize, GridSize];

        string map = TileGridSerialized;
        string entities = EntityGridSerialized;

        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                TileGrid[i, j] = map[0];
                EntityGrid[i, j] = entities[0];

                map = map.Remove(0, 1);
                entities = entities.Remove(0, 1);
            }
        }
    }

    public override string ToString()
    {
        string str = string.Empty;
        str += "Tile Grid\n";
        str += GameUtilities.ArrayToString(TileGrid);

        str += "Entity Grid\n";
        str += GameUtilities.ArrayToString(EntityGrid);

        return str;
    }
}