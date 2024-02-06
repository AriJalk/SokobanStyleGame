
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Manager : MonoBehaviour
{
    private const int GRID_SIZE = 5;
    private const float OFFSET = 0.02f;
    private UnityEvent<IActorCommands> inputEvents;

    private TileType tileType;

    private PrefabManager prefabManager;
    private ActorManager actorManager;

    private MapManager mapManager; 

    public Transform MapTransform;
    public Transform UnactiveObjects;

    private void Awake()
    {
        inputEvents = new UnityEvent<IActorCommands>();

        prefabManager = new PrefabManager(UnactiveObjects);
        prefabManager.LoadAndRegisterGameObject("Cube", GRID_SIZE * GRID_SIZE);
        prefabManager.LoadAndRegisterGameObject("Player", 5);
        prefabManager.LoadAndRegisterGameObject("Tile", GRID_SIZE * GRID_SIZE);

        //prefabManager.RegisterPrefab<CubeObject>(Resources.Load<GameObject>("Cube"), GRID_SIZE * GRID_SIZE);
        tileType = new TileType("Basic");
        mapManager = new MapManager();
        mapManager.SetMap(GRID_SIZE, GRID_SIZE);
        actorManager = new ActorManager(inputEvents, GRID_SIZE, prefabManager, MapTransform);


        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                TileObject tileObject = prefabManager.RetrievePoolObject("Tile").GetComponent<TileObject>();
                tileObject.transform.SetParent(MapTransform);
                tileObject.GridPosition = new Vector2Int(i, j);
                tileObject.TileType = tileType;
                tileObject.transform.localScale = Vector3.one;
                tileObject.transform.localPosition = new Vector3(i + i * OFFSET, 0, j + j * OFFSET);
                mapManager.MapGrid[i, j] = tileObject;
            }
        }

        mapManager.MapGrid[3, 3].ObjectSlot = prefabManager.RetrievePoolObject("Cube");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {

        }

    }

    private void OnDestroy()
    {
        inputEvents.RemoveAllListeners();
    }

    private void ExecuteNextStep()
    {
       
    }
}
