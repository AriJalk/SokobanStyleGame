using UnityEngine;

public class GoalTileType : TileType
{
    public GameColors Color { get; private set; }
    public GoalTileType(GameColors color) : base(ActorTypeEnum.GoalTile)
    {
        Color = color;
    }

    public void SetGoalColor(ActorObject actor)
    {
        SpriteRenderer spriteRenderer = actor.transform.GetChild(0).Find("GoalColor").GetComponent<SpriteRenderer>();
        spriteRenderer.color = GameUtilities.GameColorToUnityRGB(Color);
    }
}