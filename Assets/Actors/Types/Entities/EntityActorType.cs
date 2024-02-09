using UnityEngine;

public class EntityActorType : ActorType
{
    public bool CanPush { get; protected set; }
    public bool CanBePushed { get; protected set; }
    public EntityActorType LinkedActorType { get; protected set; }

    public EntityActorType(ActorTypeEnum type, bool canPush, bool canBePushed) : base(type)
    {
        CanPush = canPush;
        CanBePushed = canBePushed;
    }

    public static void SetLinkedTypes(EntityActorType typeA,  EntityActorType typeB)
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