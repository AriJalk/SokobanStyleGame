public class ActorType
{
    public string Name { get; private set; }

    public ActorType(string name)
    {
        Name = name;
    }

    public virtual void ExecuteNextStep()
    {
        return;
    }
}