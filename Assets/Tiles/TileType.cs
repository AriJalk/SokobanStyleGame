public class TileType
{
    public string Name { get; private set; }

    public TileType(string name)
    {
        Name = name;
    }

    public virtual void OnExecute()
    {
        return;
    }

    public virtual void OnActorMoveIn(ActorObject actor)
    {
        return;
    }
}