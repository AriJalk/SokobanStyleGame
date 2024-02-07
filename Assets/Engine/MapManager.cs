﻿using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    private ActorManager actorManager;
    private Transform mapLayerTransform;
    private PrefabManager prefabManager;
    private int gridSizeX;
    private int gridSizeY;
    private float offset;

    public ActorObject[,] MapGrid { get; private set; }
    public Dictionary<BorderStruct, BorderObject> Borders { get; private set; }

    public MapManager(GameManager gameManager)
    {
        actorManager = gameManager.ActorManager;
        mapLayerTransform = gameManager.MapLayerTransform;
        gridSizeX = GameManager.GRID_SIZE;
        gridSizeY = GameManager.GRID_SIZE;
        offset = GameManager.OFFSET;
        MapGrid = new ActorObject[gridSizeX, gridSizeY];
        Borders = new Dictionary<BorderStruct, BorderObject>();
        prefabManager = gameManager.PrefabManager;
    }


    public ActorObject GetTile(Vector2Int position)
    {
        if (GameUtilities.IsPositionInBounds(MapGrid, position))
        {
            return MapGrid[position.x, position.y];
        }
        return null;
    }

    public void AddTileToMap(Vector2Int position, string type)
    {
        if (MapGrid[position.x, position.y] == null)
        {
            ActorType tileType = actorManager.ActorTypes[type];
            ActorObject tile = actorManager.CreateNewActor(actorManager.ActorTypes[type]);
            if (tile != null)
            {
                tile.SetGamePosition(position);
                tile.transform.SetParent(mapLayerTransform);
                tile.transform.localPosition = new Vector3(position.x + position.x * offset, 0, position.y + position.y * offset);
                tile.transform.localScale = Vector3.one;
                MapGrid[position.x, position.y] = tile;
            }
        }
    }

    public void CreateBorder(ActorObject tileA, ActorObject tileB)
    {
        if (tileA.ActorType is TileType && tileB.ActorType is TileType)
        {
            BorderObject border = new GameObject("Border").AddComponent<BorderObject>();
            BorderStruct borderStruct = new BorderStruct(tileA, tileB);
            border.Initialize(borderStruct);
            Borders.Add(borderStruct, border);
            //Attach type model
            GameObject model = prefabManager.RetrievePoolObject("Border");
            GameUtilities.SetParentAndResetPosition(model.transform, border.transform);

            border.transform.SetParent(mapLayerTransform);
            Vector2Int difference = tileA.GamePosition - tileB.GamePosition;
            if (difference.x == 0)
            {
                border.transform.Rotate(Vector3.up * 90f);
            }
            Vector3 position = new Vector3();
            position.x = (tileA.transform.localPosition.x + tileB.transform.localPosition.x) / 2f;
            position.z = (tileA.transform.localPosition.z + tileB.transform.localPosition.z) / 2f;
            border.transform.localPosition = position;
            border.transform.localScale = Vector3.one;
        }
    }

}