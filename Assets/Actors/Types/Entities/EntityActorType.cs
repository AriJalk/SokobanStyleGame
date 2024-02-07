public class EntityActorType : ActorType
{
    public bool CanPush { get; protected set; }

    public EntityActorType(string name, bool canPush) 
    {
        Name = name;
        CanPush = canPush;
    }

    public void SetCanPush(bool canPush)
    {
        CanPush = canPush;
    }
}