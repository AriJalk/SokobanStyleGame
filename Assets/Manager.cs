
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Manager : MonoBehaviour
{
    public const int GRID_SIZE = 5;
    public const float OFFSET = 0.02f;
    public UnityEvent<IActorCommands> InputEvents;

    private TileType tileType;

    public PrefabManager PrefabManager { get; private set; }
    public ActorManager ActorManager { get; private set; }

    public MapManager MapManager;

    public Transform MapLayerTransform;
    public Transform ActorLayerTransform;
    public Transform UnactiveObjects;

    private void Awake()
    {
        InputEvents = new UnityEvent<IActorCommands>();

        PrefabManager = new PrefabManager(UnactiveObjects);
        PrefabManager.LoadAndRegisterGameObject("Cube", GRID_SIZE * GRID_SIZE);
        PrefabManager.LoadAndRegisterGameObject("Player", 5);
        PrefabManager.LoadAndRegisterGameObject("Tile", GRID_SIZE * GRID_SIZE);
        PrefabManager.LoadAndRegisterGameObject("Sphere", 10);
        PrefabManager.LoadAndRegisterGameObject("Border", 100);

        //PrefabManager.RegisterPrefab<CubeObject>(Resources.Load<GameObject>("Cube"), GRID_SIZE * GRID_SIZE);
        tileType = new TileType("Basic");
        MapManager = new MapManager();
        MapManager.SetMap(GRID_SIZE, GRID_SIZE);
        ActorManager = new ActorManager(this);


        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                TileObject tileObject = PrefabManager.RetrievePoolObject("Tile").GetComponent<TileObject>();
                tileObject.transform.SetParent(MapLayerTransform);
                tileObject.GridPosition = new Vector2Int(i, j);
                tileObject.TileType = tileType;
                tileObject.transform.localScale = Vector3.one;
                tileObject.transform.localPosition = new Vector3(i + i * OFFSET, 0, j + j * OFFSET);
                MapManager.MapGrid[i, j] = tileObject;
            }
        }
        ActorObject player = ActorManager.CreateNewActor(ActorManager.ActorTypes["Player"]);
        ActorManager.AddActorToTile(player, MapManager.MapGrid[4, 4]);

        ActorObject cube = ActorManager.CreateNewActor(ActorManager.ActorTypes["Cube"]);
        ActorManager.AddActorToTile(cube, MapManager.MapGrid[2, 2]);

        ActorObject sphere = ActorManager.CreateNewActor(ActorManager.ActorTypes["Sphere"]);
        ActorManager.AddActorToTile(sphere, MapManager.MapGrid[1, 1]);

        CreateBorder(MapManager.MapGrid[0, 0], MapManager.MapGrid[1, 0]);
        CreateBorder(MapManager.MapGrid[0, 0], MapManager.MapGrid[0, 1]);

        CreateBorder(MapManager.MapGrid[2, 2], MapManager.MapGrid[2, 3]);
        CreateBorder(MapManager.MapGrid[2, 2], MapManager.MapGrid[2, 1]);
    }

    private void CreateBorder(TileObject tileA, TileObject tileB)
    {
        BorderStruct pair = new BorderStruct(tileA, tileB);
        if (pair.IsBorderValid())
        {
            BorderObject border = PrefabManager.RetrievePoolObject("Border").GetComponent<BorderObject>();
            border.Initialize(pair, this);
            MapManager.Borders.Add(pair, border);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {

        }

    }

    private void OnDestroy()
    {
        InputEvents.RemoveAllListeners();
    }

    private void ExecuteNextStep()
    {

    }
}
