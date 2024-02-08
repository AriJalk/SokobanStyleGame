public class GoalTileType : TileType
{
    public GameColors Color { get; private set; }
    public GoalTileType(GameColors color) : base(ActorTypeEnum.GoalTile)
    {
        Color = color;
    }
}