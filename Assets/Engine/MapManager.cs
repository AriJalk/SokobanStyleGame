using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles map and tile logic
/// </summary>
public class MapManager
{
    private ActorManager actorManager;
    private Transform mapLayerTransform;
    private PrefabManager prefabManager;
    private int gridSizeX;
    private int gridSizeY;
    private float offset;

    private Dictionary<ActorTypeEnum, TileType> tileTypes;
    private Dictionary<Color, GoalTileType> goalTypes;



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
        goalTypes = new Dictionary<Color, GoalTileType>();
        tileTypes = new Dictionary<ActorTypeEnum, TileType>();


        TileType tileType = new TileType(ActorTypeEnum.BasicTile);
        tileTypes.Add(tileType.TypeName, tileType);

        GoalTileType goalTileType = new GoalTileType(GameColors.Red);
        goalTypes.Add(Color.red, goalTileType);

        goalTileType = new GoalTileType(GameColors.Blue);
        goalTypes.Add(Color.blue, goalTileType);
    }


    public ActorObject GetTile(Vector2Int position)
    {
        if (GameUtilities.IsPositionInBounds(MapGrid, position))
        {
            return MapGrid[position.x, position.y];
        }
        return null;
    }

    public void AddTileToMap(Vector2Int position, ActorTypeEnum type)
    {
        if (MapGrid[position.x, position.y] == null)
        {
            ActorType tileType = tileTypes[type];
            ActorObject tile = actorManager.CreateNewActor(tileTypes[type]);
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



    public void SetTileToMap(Vector2Int position, ActorObject tile)
    {
        tile.SetGamePosition(position);
        tile.transform.SetParent(mapLayerTransform);
        tile.transform.localPosition = new Vector3(position.x + position.x * offset, 0, position.y + position.y * offset);
        tile.transform.localScale = Vector3.one;
        MapGrid[position.x, position.y] = tile;
    }

    public ActorObject AddGoalTileToMap(Vector2Int position, Color color)
    {
        if (MapGrid[position.x, position.y] == null)
        {
            GoalTileType tileType = goalTypes[color];
            ActorObject tile = actorManager.CreateNewActor(goalTypes[color]);
            if (tile != null)
            {
                tile.SetGamePosition(position);
                tile.transform.SetParent(mapLayerTransform);
                tile.transform.localPosition = new Vector3(position.x + position.x * offset, 0, position.y + position.y * offset);
                tile.transform.localScale = Vector3.one;
                MapGrid[position.x, position.y] = tile;
                tileType.SetGoalColor(tile);
                return tile;
            }
        }
        return null;
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
            GameObject model = prefabManager.RetrievePoolObject(ActorTypeEnum.Border);
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