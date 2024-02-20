public enum GameState
{
    Menu,
    BuiltIn,
    Custom,
    Edit,
    GameLevel,
}
public enum GameDirection
{
    Left,
    Right,
    Up,
    Down,
    Neutral,
}

public enum GameActions
{
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,

    Undo,
    Reset,

    Null,
}

public enum GameColors
{
    Red,
    Green,
    Blue,
}

public enum ActorTypeEnum
{
    BasicTile,
    Border,
    Cube,
    CubeRed,
    CubeBlue,
    GoalTile,
    GoalTileRed,
    Player,
    Sphere,
}

public enum TileTypeEnum
{
    Empty,
    BasicTile,
    RedGoalTile,
    BlueGoalTile,
}

public enum EntityTypeEnum
{
    Player,
    RedCube,
    BlueCube,
    Sphere,
}