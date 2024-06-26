
using SPG.LevelEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles game scene building and gameplay-flow
/// </summary>
public class GameManager : MonoBehaviour
{
    public UnityEvent SolvedEvent;

    public const float OFFSET = 0.04f;

    private GameInputManager gameInputManager;

    private Stack<GameTurn> turnStack;

    private List<TileObject> goalTiles;

    private LevelUserInterface userInterface;


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
        if(StaticManager.GameState == GameState.GameLevel)
            gameInputManager.Listen();
        
    }
    private void OnDestroy()
    {
        gameInputManager.ActionTriggeredEvent.RemoveListener(CreateCommand);
        SolvedEvent.RemoveAllListeners();
    }

    /// <summary>
    /// Initialize game scene from a LevelStruct
    /// </summary>
    /// <param name="levelStruct"></param>
    public void InitializeGameScene(LevelStruct levelStruct)
    {
        StaticManager.GameState = GameState.GameLevel;
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
        userInterface = GameObject.Find("UserInterface").GetComponent<LevelUserInterface>();
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

    /// <summary>
    /// Convert LevelStruct to GameObjects in the world
    /// </summary>
    /// <param name="levelStruct"></param>
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
                    // Basic Tile
                    case 't':
                        TileObject tile = MapManager.AddTileToMap(new Vector2Int(i, j), ActorTypeEnum.BasicTile);
                        //Debug.Log($"TILE {i},{j} created");
                        break;
                    // Red Goal
                    case 'r':
                        goalTiles.Add(MapManager.AddGoalTileToMap(new Vector2Int(i, j), Color.red));
                        break;
                    // Blue Goal
                    case 'b':
                        goalTiles.Add(MapManager.AddGoalTileToMap(new Vector2Int(i, j), Color.blue));
                        break;
                    // Empty
                    case 'x':
                    default:
                        break;
                }
                switch (levelStruct.EntityGrid[i, j])
                {
                    // Player
                    case 'p':
                        ActorObject player = ActorManager.CreateNewActor<ActorObject>(ActorManager.ActorTypes[ActorTypeEnum.Player]);
                        ActorManager.AddActorToTile(player, MapManager.MapGrid[i, j]);
                        ActorManager.PlayableActors.Add(player);
                        break;
                    // Blue Cube
                    case 'b':
                        ActorObject blueCube = ActorManager.CreateNewCube(Color.blue);
                        ActorManager.AddActorToTile(blueCube, MapManager.MapGrid[i, j]);
                        break;
                    // Red Cube
                    case 'r':
                        ActorObject redCube = ActorManager.CreateNewCube(Color.red);
                        ActorManager.AddActorToTile(redCube, MapManager.MapGrid[i, j]);
                        break;
                    // Sphere
                    case 's':
                        ActorObject sphere = ActorManager.CreateNewActor<ActorObject>(ActorManager.ActorTypes[ActorTypeEnum.Sphere]);
                        ActorManager.AddActorToTile(sphere, MapManager.MapGrid[i, j]);
                        break;
                    // Empty
                    case 'x':
                    default:
                        break;
                }
            }
        }
        // Build border
        foreach (LevelEditorBorderStruct border in levelStruct.BorderList)
        {

            MapManager.CreateBorder(MapManager.GetTile(border.PositionA), MapManager.GetTile(border.PositionB));
        }
    }


    /// <summary>
    /// Create simultanious move command for every player registered under playable-actors
    /// </summary>
    /// <param name="direction"></param>
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
        // Add set to command list so undo is possible and execute
        CommandSet playerSet = new CommandSet(commandList);
        playerSet.ExecuteSet();
        commandSets.Add(playerSet);

        // Add commands to objects pushed by initial push
        List<ActorObject> pushedActors = playerSet.GetPushedActorObjects();
        List<EntityActorTypeBase> pushedTypes = new List<EntityActorTypeBase>();
        foreach (ActorObject pushedActor in pushedActors)
        {
            if (pushedActor != null)
            {
                if (!pushedTypes.Contains(pushedActor.ActorType as EntityActorTypeBase))
                {
                    pushedTypes.Add(pushedActor.ActorType as EntityActorTypeBase);
                }
            }
        }
        // Add commands to linked actors connected by type to initial pushed type for linked movement
        commandList = new List<IActorCommands>();
        foreach (EntityActorTypeBase pushedType in pushedTypes)
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
        // If change was made to game state save the contributing commands for undo
        if (turn.IsTurnProductive() == true)
            turnStack.Push(turn);
        CheckWinCondition();
    }

    /// <summary>
    /// Check if all goal tiles have matching cubes on them at the same time
    /// </summary>
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
