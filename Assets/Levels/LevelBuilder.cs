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
            { 'x', 'R', 't', 't', 't' },
            { 't', 't', 't', 't', 't' },
            { 't', 't', 't', 't', 't' },
            { 't', 't', 't', 't', 't' },
            { 't', 'B', 't', 't', 't' },
        };

        char[,] entities = {
            { 'x', 'x', 'p', 'x', 'x' },
            { 'x', 'x', 's', 'x', 'x' },
            { 'x', 'x', 'r', 'x', 'x' },
            { 'x', 'x', 'x', 'b', 'x' },
            { 'x', 'x', 'x', 'x', 'p' },
        };

        List<Tuple<Vector2Int, Vector2Int>> tupleList = new List<Tuple<Vector2Int, Vector2Int>>();

        Tuple<Vector2Int, Vector2Int> tuple = new Tuple<Vector2Int, Vector2Int>(new Vector2Int(2, 2), new Vector2Int(2, 3));
        tupleList.Add(tuple);

        tuple = new Tuple<Vector2Int, Vector2Int>(new Vector2Int(2, 2), new Vector2Int(2, 1));
        tupleList.Add(tuple);

        Levels.Add(new LevelStruct(tiles, entities, tupleList));

        ////////////////////////////
        ///
        tiles = new char[,]{
            { 'x', 'R', 't', 't', },
            { 't', 't', 't', 't', },
            { 't', 't', 't', 't', },
            { 't', 't', 't', 't', },
            { 't', 'B', 't', 't', },
        };
        entities = new char[,]{
            { 'x', 'x', 'p', 'x', },
            { 'x', 'x', 's', 'x', },
            { 'x', 'x', 'r', 'x', },
            { 'x', 'x', 'b', 'x', },
            { 'x', 'x', 'x', 'p', },
        };

        tupleList = new List<Tuple<Vector2Int, Vector2Int>>();

        tuple = new Tuple<Vector2Int, Vector2Int>(new Vector2Int(2, 2), new Vector2Int(2, 3));
        tupleList.Add(tuple);
        tuple = new Tuple<Vector2Int, Vector2Int>(new Vector2Int(2, 2), new Vector2Int(2, 1));
        tupleList.Add(tuple);

        Levels.Add(new LevelStruct(tiles, entities, tupleList));
    }
}