using SPG.LevelEditor;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class LevelBuilder
{
    public List<LevelStruct> Levels { get; private set; }

    public LevelBuilder()
    {
        Levels = new List<LevelStruct>();
        char[,] tiles = {
            { 'x', 'r', 't', 't', 't' },
            { 't', 't', 't', 't', 't' },
            { 't', 't', 't', 't', 't' },
            { 't', 't', 't', 't', 't' },
            { 't', 'b', 't', 't', 't' },
        };

        char[,] entities = {
            { 'x', 'x', 'p', 'x', 'x' },
            { 'x', 'x', 's', 'x', 'x' },
            { 'x', 'x', 'r', 'x', 'x' },
            { 'x', 'x', 'x', 'b', 'x' },
            { 'x', 'x', 'x', 'x', 'p' },
        };

        List<LevelEditorBorderStruct> borderList = new List<LevelEditorBorderStruct>();

        LevelEditorBorderStruct borderStruct = new LevelEditorBorderStruct(new Vector2Int(2, 2), new Vector2Int(2, 3));
        borderList.Add(borderStruct);

        borderStruct = new LevelEditorBorderStruct(new Vector2Int(2, 2), new Vector2Int(2, 1));
        borderList.Add(borderStruct);

        Levels.Add(new LevelStruct(tiles, entities, new List<LevelEditorBorderStruct>(borderList)));

        ////////////////////////////
        ///
        tiles = new char[,]{
            { 'x', 'r', 't', 't', },
            { 't', 't', 't', 't', },
            { 't', 't', 't', 't', },
            { 't', 't', 't', 't', },
            { 't', 'b', 't', 't', },
        };
        entities = new char[,]{
            { 'x', 'x', 'p', 'x', },
            { 'x', 'x', 's', 'x', },
            { 'x', 'x', 'r', 'x', },
            { 'x', 'x', 'b', 'x', },
            { 'x', 'x', 'x', 'p', },
        };

        borderList = new List<LevelEditorBorderStruct>();

        borderStruct = new LevelEditorBorderStruct(new Vector2Int(2, 2), new Vector2Int(2, 3));
        borderList.Add(borderStruct);
        borderStruct = new LevelEditorBorderStruct(new Vector2Int(2, 2), new Vector2Int(2, 1));
        borderList.Add(borderStruct);

        Levels.Add(new LevelStruct(tiles, entities, new List<LevelEditorBorderStruct>(borderList)));
    }
}