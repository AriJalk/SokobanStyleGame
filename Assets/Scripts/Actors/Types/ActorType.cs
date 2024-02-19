using System.Collections.Generic;

/// <summary>
/// Defines shared behavior across all actors with the same type
/// </summary>
public class ActorType
{
    public ActorTypeEnum TypeName { get; protected set; }
    /// <summary>
    /// Actors that belong to the type instance
    /// </summary>
    public List<ActorObject> ActorObjectList { get; protected set; }

    public ActorType(ActorTypeEnum type)
    {
        TypeName = type;
        ActorObjectList = new List<ActorObject>();
    }

    public virtual void ExecuteNextStep()
    {
        return;
    }

    public void SetName(ActorTypeEnum type)
    {
        TypeName = type;
    }

    public void SetType(ActorTypeEnum type)
    {
        TypeName = type;
    }
}