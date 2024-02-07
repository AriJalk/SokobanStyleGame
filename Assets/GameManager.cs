
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public const int GRID_SIZE = 5;
    public const float OFFSET = 0.02f;

    private TileType tileType;
    private ActorObject player;

    private GameInputManager gameInputManager;

    private Stack<TurnCommands> commandStack;


    public PrefabManager PrefabManager { get; private set; }
    public ActorManager ActorManager { get; private set; }

    public MapManager MapManager;

    public Transform MapLayerTransform;
    public Transform ActorLayerTransform;
    public Transform UnactiveObjects;



    private void Awake()
    {
        gameInputManager = new GameInputManager();
        gameInputManager.ActionTriggeredEvent.AddListener(CreateCommand);

        commandStack = new Stack<TurnCommands>();

        PrefabManager = new PrefabManager(UnactiveObjects);
        PrefabManager.LoadAndRegisterGameObject("Cube", GRID_SIZE * GRID_SIZE);
        PrefabManager.LoadAndRegisterGameObject("Player", 5);
        PrefabManager.LoadAndRegisterGameObject("BasicTile", GRID_SIZE * GRID_SIZE);
        PrefabManager.LoadAndRegisterGameObject("Sphere", 10);
        PrefabManager.LoadAndRegisterGameObject("Border", 100);



        ActorManager = new ActorManager(this);
        MapManager = new MapManager(this);

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                MapManager.AddTileToMap(new Vector2Int(i, j), "BasicTile");
            }
        }  

        //TODO: naming auto in class
        ActorManager.ActorTypes["Cube"].SetResourceName("Cube");
        //TODO: naming auto in class
        ActorManager.ActorTypes["Sphere"].SetResourceName("Sphere");


        ActorObject player = ActorManager.CreateNewActor(ActorManager.ActorTypes["Player"]);
        ActorManager.AddActorToTile(player, MapManager.MapGrid[4, 4]);
        this.player = player;

        ActorObject cube = ActorManager.CreateNewActor(ActorManager.ActorTypes["Cube"]);
        ActorManager.AddActorToTile(cube, MapManager.MapGrid[2, 2]);



        cube = ActorManager.CreateNewActor(ActorManager.ActorTypes["Cube"]);
        ActorManager.AddActorToTile(cube, MapManager.MapGrid[3, 3]);

        ActorObject sphere = ActorManager.CreateNewActor(ActorManager.ActorTypes["Sphere"]);
        ActorManager.AddActorToTile(sphere, MapManager.MapGrid[1, 1]);

        MapManager.CreateBorder(MapManager.MapGrid[0, 0], MapManager.MapGrid[1, 0]);
        MapManager.CreateBorder(MapManager.MapGrid[0, 0], MapManager.MapGrid[0, 1]);

        MapManager.CreateBorder(MapManager.MapGrid[2, 2], MapManager.MapGrid[2, 3]);
        MapManager.CreateBorder(MapManager.MapGrid[2, 2], MapManager.MapGrid[2, 1]);
    }

    private void Update()
    {
        gameInputManager.Listen();

    }

    private void OnDestroy()
    {
        gameInputManager.ActionTriggeredEvent.RemoveListener(CreateCommand);
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

    public void CreateCommand(GameActions action)
    {
        switch (action)
        {
            case GameActions.MoveUp:
                CreateMoveCommand(player, GameDirection.Up);
                break;
            case GameActions.MoveDown:
                CreateMoveCommand (player, GameDirection.Down);
                break;
            case GameActions.MoveLeft:
                CreateMoveCommand(player, GameDirection.Left);
                break;
            case GameActions.MoveRight:
                CreateMoveCommand(player, GameDirection.Right);
                break;
            case GameActions.Undo:
                UndoState();
                break;
            case GameActions.Reset:
                while(commandStack.Count > 0)
                {
                    commandStack.Pop().UndoCommands();
                }
                break;
            default:
                break;
        }
    }
}
