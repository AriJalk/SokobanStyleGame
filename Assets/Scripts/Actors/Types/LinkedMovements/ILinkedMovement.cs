/// <summary>
/// Defines linked movement behavior in response to linked actor action
/// </summary>
public interface ILinkedMovement
{
    public GameDirection GetLinkedMovementVector(GameDirection direction);
}