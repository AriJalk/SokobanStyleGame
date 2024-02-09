using System.Collections.Generic;

public class ActorType
{
    public ActorTypeEnum TypeName { get; protected set; }
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