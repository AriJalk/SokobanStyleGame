public class GoalTileType : TileType
{
    public const string NAME = "_Goal Tile";
    public GameColors Color { get; private set; }
    public GoalTileType(GameColors color) : base()
    {
        Name = Color + NAME;
        Color = color;
    }
}