
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

    public const float INPUT_FREQUENCY = 0.1f;
    public const float INPUT_REPEAT_DELAY = 0.2f;

    public UnityEvent<IActorCommands> InputEvents;

    private TileType tileType;
    private ActorObject player;

    private Stack<TurnCommands> commandStack;

    private float inputHoldTimer;
    private float inputRepeatDelayTimer;
    private KeyCode lastKeyPress;
    private int repeats;

    public PrefabManager PrefabManager { get; private set; }
    public ActorManager ActorManager { get; private set; }

    public MapManager MapManager;

    public Transform MapLayerTransform;
    public Transform ActorLayerTransform;
    public Transform UnactiveObjects;

    private void OnKeyDown(KeyCode keyCode)
    {
        lastKeyPress = keyCode;
        inputHoldTimer = 0f;
        inputRepeatDelayTimer = 0f;
        repeats = 0;
        CreateMoveCommand(keyCode);
    }

    private void OnKeyHeld(KeyCode keyCode)
    {
        if (lastKeyPress == keyCode)
        {
            if (inputRepeatDelayTimer < INPUT_REPEAT_DELAY)
            {
                inputRepeatDelayTimer += Time.deltaTime;
            }
            else
            {
                inputHoldTimer += Time.deltaTime + repeats / 5000f;
            }
            if (inputHoldTimer > INPUT_FREQUENCY)
            {
                inputHoldTimer = 0f;
                CreateMoveCommand(keyCode);
                repeats++;
            }
        }
    }



    private void Awake()
    {
        InputEvents = new UnityEvent<IActorCommands>();
        commandStack = new Stack<TurnCommands>();

        PrefabManager = new PrefabManager(UnactiveObjects);
        PrefabManager.LoadAndRegisterGameObject("Cube", GRID_SIZE * GRID_SIZE);
        PrefabManager.LoadAndRegisterGameObject("Player", 5);
        PrefabManager.LoadAndRegisterGameObject("Tile", GRID_SIZE * GRID_SIZE);
        PrefabManager.LoadAndRegisterGameObject("Sphere", 10);
        PrefabManager.LoadAndRegisterGameObject("Border", 100);

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
        this.player = player;

        ActorObject cube = ActorManager.CreateNewActor(ActorManager.ActorTypes["Cube"]);
        ActorManager.AddActorToTile(cube, MapManager.MapGrid[2, 2]);

        cube = ActorManager.CreateNewActor(ActorManager.ActorTypes["Cube"]);
        ActorManager.AddActorToTile(cube, MapManager.MapGrid[3, 3]);

        ActorObject sphere = ActorManager.CreateNewActor(ActorManager.ActorTypes["Sphere"]);
        ActorManager.AddActorToTile(sphere, MapManager.MapGrid[1, 1]);

        CreateBorder(MapManager.MapGrid[0, 0], MapManager.MapGrid[1, 0]);
        CreateBorder(MapManager.MapGrid[0, 0], MapManager.MapGrid[0, 1]);

        CreateBorder(MapManager.MapGrid[2, 2], MapManager.MapGrid[2, 3]);
        CreateBorder(MapManager.MapGrid[2, 2], MapManager.MapGrid[2, 1]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            OnKeyDown(KeyCode.UpArrow);
        else if (Input.GetKey(KeyCode.UpArrow))
            OnKeyHeld(KeyCode.UpArrow);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            OnKeyDown(KeyCode.DownArrow);
        else if (Input.GetKey(KeyCode.DownArrow))
            OnKeyHeld(KeyCode.DownArrow);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            OnKeyDown(KeyCode.LeftArrow);
        else if (Input.GetKey(KeyCode.LeftArrow))
            OnKeyHeld(KeyCode.LeftArrow);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            OnKeyDown(KeyCode.RightArrow);
        else if (Input.GetKey(KeyCode.RightArrow))
            OnKeyHeld(KeyCode.RightArrow);

        if (Input.GetKeyDown(KeyCode.Z))
            OnKeyDown(KeyCode.Z);
        else if (Input.GetKey(KeyCode.Z))
            OnKeyHeld(KeyCode.Z);

    }

    private void OnDestroy()
    {
        InputEvents.RemoveAllListeners();
    }

    private void CreateBorder(TileObject tileA, TileObject tileB)
    {
        BorderStruct pair = new BorderStruct(tileA, tileB);
        if (pair.IsBorderValid())
        {
            BorderObject border = PrefabManager.RetrievePoolObject("Border").GetComponent<BorderObject>();
            border.Initialize(pair, this);
            MapManager.Borders.Add(pair, border);
            //pair = new BorderStruct(tileB, tileA);
            //MapManager.Borders.Add(pair, border);
        }
    }

    private void CreateMoveCommand(ActorObject actor, GameDirection direction)
    {
        ActorMoveCommand command = new ActorMoveCommand(actor, direction, MapManager, ActorManager);
        TurnCommands turn = new TurnCommands();
        turn.Commands.Add(command);
        turn.ExecuteCommands();
        //Discard empty command container if no action can be performed
        if (turn.Commands.Count > 0)
            commandStack.Push(turn);
    }

    private void CreateMoveCommand(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.UpArrow:
                CreateMoveCommand(player, GameDirection.Up);
                break;
            case KeyCode.DownArrow:
                CreateMoveCommand(player, GameDirection.Down);
                break;
            case KeyCode.LeftArrow:
                CreateMoveCommand(player, GameDirection.Left);
                break;
            case KeyCode.RightArrow:
                CreateMoveCommand(player, GameDirection.Right);
                break;
            case KeyCode.Z:
                UndoState();
                break;
            default:
                break;
        }
    }

    private void ExecuteNextStep()
    {

    }
    private void UndoState()
    {
        if (commandStack.Count > 0)
        {
            TurnCommands commands = commandStack.Pop();
            commands.UndoCommands();
        }
    }


}
