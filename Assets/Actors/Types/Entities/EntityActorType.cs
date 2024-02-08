public class EntityActorType : ActorType
{
    public bool CanPush { get; protected set; }
    public bool CanBePushed { get; protected set; }

    public EntityActorType(ActorTypeEnum type, bool canPush, bool canBePushed) : base(type)
    {
        CanPush = canPush;
        CanBePushed = canBePushed;
    }

    public void SetCanPush(bool canPush)
    {
        CanPush = canPush;
    }

    public void SetCanBePushed(bool canBePushed)
    {
        CanBePushed = canBePushed;
    }
}