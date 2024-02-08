public class EntityActorType : ActorType
{
    public bool CanPush { get; protected set; }

    public EntityActorType(ActorTypeEnum type, bool canPush) : base(type)
    {
        CanPush = canPush;
    }

    public void SetCanPush(bool canPush)
    {
        CanPush = canPush;
    }
}