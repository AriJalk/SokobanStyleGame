
using SPG.LevelEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent SolvedEvent;

    public const float OFFSET = 0.04f;

    private GameInputManager gameInputManager;

    private Stack<GameTurn> turnStack;

    private List<TileObject> goalTiles;

    private UserInterface userInterface;


    // Transforms to load
    public Transform MapLayerTransform {  get; private set; }
    public Transform ActorLayerTransform { get; private set; }
    public Transform UnactiveObjects {  get; private set; }

    public MapManager MapManager {  get; private set; }
    public PrefabManager PrefabManager { get; private set; }
    public ActorManager ActorManager { get; private set; }
    public int Grid_Size { get; private set; }





    private void Awake()
    {
        SolvedEvent = new UnityEvent();
        gameInputManager = new GameInputManager();
        gameInputManager.ActionTriggeredEvent.AddListener(CreateCommand);

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        gameInputManager.Listen();
        
    }
    private void OnDestroy()
    {
        gameInputManager.ActionTriggeredEvent.RemoveListener(CreateCommand);
        SolvedEvent.RemoveAllListeners();
    }

    public void InitializeGameScene(LevelStruct levelStruct)
    {
        turnStack = new Stack<GameTurn>();
        Grid_Size = levelStruct.GridSize;
        GameObject world = GameObject.Find("World");
        if(world == null)
        {
            Debug.Log("World not found");
            return;
        }

        
        MapLayerTransform = world.transform.Find("MapLayer");
        ActorLayerTransform = world.transform.Find("ActorLayer").transform;
        UnactiveObjects = GameObject.Find("UnactiveObjects").transform;
        userInterface = GameObject.Find("UserInterface").GetComponent<UserInterface>();
        userInterface.Initialize(SolvedEvent);
        PrefabManager = new PrefabManager(UnactiveObjects);

        ActorManager = new ActorManager(this);
        MapManager = new MapManager(this);

        if (MapLayerTransform == null || ActorLayerTransform == null || UnactiveObjects == null || userInterface == null)
        {
            Debug.Log("Missing Components");
            return;
        }

        PrefabManager.LoadAndRegisterGameObject(ActorTypeEnum.Cube, Grid_Size * Grid_Size);
        PrefabManager.LoadAndRegisterGameObject(ActorTypeEnum.Player, 5);
        PrefabManager.LoadAndRegisterGameObject(ActorTypeEnum.BasicTile, Grid_Size * Grid_Size);
        PrefabManager.LoadAndRegisterGameObject(ActorTypeEnum.Sphere, 10);
        PrefabManager.LoadAndRegisterGameObject(ActorTypeEnum.Border, Grid_Size * Grid_Size);
        PrefabManager.LoadAndRegisterGameObject(ActorTypeEnum.GoalTile, 5);

        BuildLevel(levelStruct);
    }


    private void BuildLevel(LevelStruct levelStruct)
    {
        goalTiles = new List<TileObject>();
        // Build Map and Entity grid
        for (int i = 0; i < levelStruct.TileGrid.GetLength(0); i++)
        {
            for (int j = 0; j < levelStruct.TileGrid.GetLength(1); j++)
            {
                switch (levelStruct.TileGrid[i, j])
                {
                    case 't':
                        TileObject tile = MapManager.AddTileToMap(new Vector2Int(i, j), ActorTypeEnum.BasicTile);
                        Debug.Log($"TILE {i},{j} created");
                        break;
                    case 'r':
                        goalTiles.Add(MapManager.AddGoalTileToMap(new Vector2Int(i, j), Color.red));
                        break;
                    case 'b':
                        goalTiles.Add(MapManager.AddGoalTileToMap(new Vector2Int(i, j), Color.blue));
                        break;
                    case 'x':
                    default:
                        break;
                }
                switch (levelStruct.EntityGrid[i, j])
                {
                    case 'p':
                        ActorObject player = ActorManager.CreateNewActor<ActorObject>(ActorManager.ActorTypes[ActorTypeEnum.Player]);
                        ActorManager.AddActorToTile(player, MapManager.MapGrid[i, j]);
                        ActorManager.PlayableActors.Add(player);
                        break;
                    case 'b':
                        ActorObject blueCube = ActorManager.CreateNewCube(Color.blue);
                        ActorManager.AddActorToTile(blueCube, MapManager.MapGrid[i, j]);
                        break;
                    case 'r':
                        ActorObject redCube = ActorManager.CreateNewCube(Color.red);
                        ActorManager.AddActorToTile(redCube, MapManager.MapGrid[i, j]);
                        break;
                    case 's':
                        ActorObject sphere = ActorManager.CreateNewActor<ActorObject>(ActorManager.ActorTypes[ActorTypeEnum.Sphere]);
                        ActorManager.AddActorToTile(sphere, MapManager.MapGrid[i, j]);
                        break;
                    case 'x':
                    default:
                        break;
                }
            }
        }
        // Build border
        foreach (LevelEditorBorderStruct border in levelStruct.BorderList)
        {
            
            Vector2Int positionA = GameUtilities.EditorToGamePosition(border.PositionA, Grid_Size);
            Vector2Int positionB = GameUtilities.EditorToGamePosition(border.PositionB, Grid_Size);
            Debug.Log($"Border Position A: {border.PositionA} B: {border.PositionB} newA: {positionA} newB{positionB}");

            MapManager.CreateBorder(MapManager.GetTile(positionA), MapManager.GetTile(positionB));
        }
    }



    private void CreateMoveCommandForAllPlayers(GameDirection direction)
    {
        List<CommandSet> commandSets = new List<CommandSet>();

        List<IActorCommands> commandList = new List<IActorCommands>();
        GameTurn turn = new GameTurn();



        //Move all regular players
        foreach (ActorObject player in ActorManager.PlayableActors)
        {
            if (player != null)
            {
                ActorMoveCommand command = new ActorMoveCommand(player, direction, MapManager, ActorManager);
                commandList.Add(command);
            }
        }
        // Add set to list
        CommandSet playerSet = new CommandSet(commandList);
        playerSet.ExecuteSet();
        commandSets.Add(playerSet);

        // Add commands to objects linked to type
        List<ActorObject> pushedActors = playerSet.GetPushedActorObjects();
        List<EntityActorType> pushedTypes = new List<EntityActorType>();
        foreach (ActorObject pushedActor in pushedActors)
        {
            if (pushedActor != null)
            {
                if (!pushedTypes.Contains(pushedActor.ActorType as EntityActorType))
                {
                    pushedTypes.Add(pushedActor.ActorType as EntityActorType);
                }
            }
        }
        commandList = new List<IActorCommands>();
        foreach (EntityActorType pushedType in pushedTypes)
        {
            GameDirection linkDirection = pushedType.GetLinkedDirection(direction);
            if (pushedType.LinkedActorType != null)
            {
                foreach (ActorObject actor in pushedType.LinkedActorType.ActorObjectList)
                {
                    ActorMoveCommand command = new ActorMoveCommand(actor, linkDirection, MapManager, ActorManager);
                    commandList.Add(command);
                }
            }
        }
        //Add commands to set and execute
        CommandSet linkedSet = new CommandSet(commandList);
        linkedSet.ExecuteSet();
        commandSets.Add(linkedSet);


        turn.CommandSets = commandSets;
        if (turn.IsTurnProductive() == true)
            turnStack.Push(turn);
        CheckWinCondition();
    }


    private void CheckWinCondition()
    {
        bool result = true;
        foreach (ActorObject goal in goalTiles)
        {
            if (goal.ActorType is GoalTileType goalType)
            {
                ActorObject actor = ActorManager.GetActor(goal.GamePosition);
                if (actor == null)
                {
                    result = false;
                    break;
                }
                if(!IsColorMatching(goal, actor))
                {
                    result = false;
                    break;
                }
            }
        }
        if (result == true)
            SolvedEvent?.Invoke();

    }

    private bool IsColorMatching(ActorObject goal, ActorObject actor)
    {
        if (goal.ActorType is GoalTileType goalType && actor.ActorType is CubeActorType cubeActorType)
        {
            if (goalType.Color == cubeActorType.Color)
                return true;
        }
        return false;
    }

    private void UndoState()
    {
        if (turnStack.Count > 0)
        {
            GameTurn commands = turnStack.Pop();
            commands.UndoAllSets();
        }
    }

    

    public void CreateCommand(GameActions action)
    {
        switch (action)
        {
            case GameActions.MoveUp:
                CreateMoveCommandForAllPlayers(GameDirection.Up);
                break;
            case GameActions.MoveDown:
                CreateMoveCommandForAllPlayers(GameDirection.Down);
                break;
            case GameActions.MoveLeft:
                CreateMoveCommandForAllPlayers(GameDirection.Left);
                break;
            case GameActions.MoveRight:
                CreateMoveCommandForAllPlayers(GameDirection.Right);
                break;
            case GameActions.Undo:
                UndoState();
                break;
            case GameActions.Reset:
                while (turnStack.Count > 0)
                {
                    turnStack.Pop().UndoAllSets();
                }
                break;
            default:
                break;
        }
    }
}
