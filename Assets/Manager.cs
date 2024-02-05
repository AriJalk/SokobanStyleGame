
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
    private PlayableActorType playableActorType;

    private PrefabManager prefabManager;
    private TileObject[,] tileGrid;
    private List<ActorObject> actors;

    public Transform MapTransform;
    public Transform ActorsTransform;
    public Transform UnactiveObjects;

    private void Awake()
    {
        inputEvents = new UnityEvent<IActorCommands>();
        actors = new List<ActorObject>();
        prefabManager = new PrefabManager(UnactiveObjects);
        prefabManager.RegisterPrefab<TileObject>(Resources.Load<GameObject>("Tile"), GRID_SIZE * GRID_SIZE);
        prefabManager.RegisterPrefab<ActorObject>(Resources.Load<GameObject>("Marine"), 5);
        prefabManager.RegisterPrefab<CubeObject>(Resources.Load<GameObject>("Cube"), GRID_SIZE * GRID_SIZE);
        tileType = new TileType("Basic");
        playableActorType = new PlayableActorType("Marine");
        playableActorType.SetInputEvents(inputEvents);

        tileGrid = new TileObject[GRID_SIZE, GRID_SIZE];

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                TileObject tileObject = prefabManager.RetrievePoolObject<TileObject>();
                tileObject.transform.SetParent(MapTransform);
                tileObject.GridPosition = new Vector2Int(i, j);
                tileObject.TileType = tileType;
                tileObject.transform.localScale = Vector3.one;
                tileObject.transform.localPosition = new Vector3(i + i * OFFSET, 0, j + j * OFFSET);
                tileGrid[i, j] = tileObject;
            }
        }
        ActorObject playable = prefabManager.RetrievePoolObject<ActorObject>();
        actors.Add(playable);
        playable.SetGamePosition(new Vector2Int(0, 0));
        playable.SetActorType(playableActorType);
        playable.transform.SetParent(ActorsTransform);
        playable.transform.localPosition = new Vector3(playable.GamePosition.x, 0, playable.GamePosition.y);
        playable.transform.localScale = Vector3.one;
        tileGrid[3, 3].ObjectSlot = prefabManager.RetrievePoolObject<CubeObject>().gameObject;
        tileGrid[2, 2].ObjectSlot = prefabManager.RetrievePoolObject<CubeObject>().gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            inputEvents?.Invoke(new ActorMoveCommand(actors[0], GameDirection.Up, 1, tileGrid));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            inputEvents?.Invoke(new ActorMoveCommand(actors[0], GameDirection.Down, 1, tileGrid));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            inputEvents?.Invoke(new ActorMoveCommand(actors[0], GameDirection.Left, 1, tileGrid));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            inputEvents?.Invoke(new ActorMoveCommand(actors[0], GameDirection.Right, 1, tileGrid));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            inputEvents?.Invoke(new ActorMoveCommand(actors[0], GameDirection.UpLeft, 1, tileGrid));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            inputEvents?.Invoke(new ActorMoveCommand(actors[0], GameDirection.UpRight, 1, tileGrid));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            inputEvents?.Invoke(new ActorMoveCommand(actors[0], GameDirection.DownLeft, 1, tileGrid));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            inputEvents?.Invoke(new ActorMoveCommand(actors[0], GameDirection.DownRight, 1, tileGrid));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            inputEvents?.Invoke(new ActorRotateCommand(actors[0], GameRotation.Clockwise));
            ExecuteNextStep();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inputEvents?.Invoke(new ActorRotateCommand(actors[0], GameRotation.CounterClockwise));
            ExecuteNextStep();
        }
    }

    private void ExecuteNextStep()
    {
        foreach (ActorObject actor in actors)
        {
            actor.ActorType.ExecuteNextStep();
            float newX = actor.GamePosition.x + actor.GamePosition.x * OFFSET;
            float newZ = actor.GamePosition.y + actor.GamePosition.y * OFFSET;
            actor.transform.localPosition = new Vector3(newX, 0, newZ);
        }
    }
}
