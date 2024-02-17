using SPG.LevelEditor;
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
    public string BorderSetSerialized;
    [SerializeField]
    public int GridSize;
    public char[,] TileGrid { get; private set; }
    public char[,] EntityGrid { get; private set; }
    public List<LevelEditorBorderStruct> BorderList { get; private set; }



    public LevelStruct(char[,] tileGrid, char[,] entityGrid, List<LevelEditorBorderStruct> borders)
    {
        TileGrid = tileGrid;
        EntityGrid = entityGrid;
        BorderList = borders;

        TileGridSerialized = string.Empty;
        EntityGridSerialized = string.Empty;
        BorderSetSerialized = string.Empty;

        GridSize = tileGrid.GetLength(0);
    }



    public void SerializeFields()
    {
        TileGridSerialized = string.Empty;
        EntityGridSerialized = string.Empty;
        BorderSetSerialized = string.Empty;
        GridSize = TileGrid.GetLength(0);
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
        for (int i = 0; i < BorderList.Count; i++)
        {
            BorderSetSerialized += JsonUtility.ToJson(BorderList[i]);
            if (i < BorderList.Count - 1)
                BorderSetSerialized += "#separator#";
        }
    }
    public void DeserializeFields()
    {
        BorderList = new List<LevelEditorBorderStruct>();
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
        // Split the serialized string using the separator
        string[] splitStringArray = BorderSetSerialized.Split(new string[] { "#separator#" }, StringSplitOptions.None);
        foreach (string json in splitStringArray)
        {
            if (json != string.Empty)
                BorderList.Add(JsonUtility.FromJson<LevelEditorBorderStruct>(json));
        }
    }

    /*public override string ToString()
    {
        string str = string.Empty;
        str += "Tile Grid\n";
        str += GameUtilities.ArrayToString(TileGrid);

        str += "Entity Grid\n";
        str += GameUtilities.ArrayToString(EntityGrid);

        return str;
    }*/
}