public class ActorType
{
    public string Name { get; protected set; }
    public string ResourceName {  get; protected set; }

    public ActorType()
    {

    }

    public ActorType(string name)
    {
        Name = name;
        ResourceName = name;
    }

    public virtual void ExecuteNextStep()
    {
        return;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetResourceName(string resourceName)
    {
        ResourceName = resourceName;
    }
}