public class ActorType
{
    public string Name { get; private set; }
    public string ResourceName {  get; protected set; }

    public ActorType(string name)
    {
        Name = name;
    }

    public virtual void ExecuteNextStep()
    {
        return;
    }

    public void SetResourceName(string resourceName)
    {
        ResourceName = resourceName;
    }
}