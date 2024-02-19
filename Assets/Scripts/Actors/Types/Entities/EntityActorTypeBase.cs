/// <summary>
/// The general entity actor (non tile) type
/// </summary>
public class EntityActorTypeBase : ActorType
{
    public bool CanPush { get; protected set; }
    public bool CanBePushed { get; protected set; }
    public EntityActorTypeBase LinkedActorType { get; protected set; }

    public EntityActorTypeBase(ActorTypeEnum type, bool canPush, bool canBePushed) : base(type)
    {
        CanPush = canPush;
        CanBePushed = canBePushed;
    }

    public static void SetLinkedTypes(EntityActorTypeBase typeA,  EntityActorTypeBase typeB)
    {
        if (typeA.LinkedActorType != null)
            typeA.LinkedActorType.LinkedActorType = null;
        if (typeB.LinkedActorType != null)
            typeB.LinkedActorType.LinkedActorType = null;
        typeA.LinkedActorType = typeB;
        typeB.LinkedActorType = typeA;
    }

    public void SetCanPush(bool canPush)
    {
        CanPush = canPush;
    }

    public void SetCanBePushed(bool canBePushed)
    {
        CanBePushed = canBePushed;
    }

    public virtual GameDirection GetLinkedDirection(GameDirection originDirection)
    {
        return originDirection;
    }
}