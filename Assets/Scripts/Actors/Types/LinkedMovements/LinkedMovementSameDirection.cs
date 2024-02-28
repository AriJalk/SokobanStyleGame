public class LinkedMovementSameDirection : ILinkedMovement
{
    public GameDirection GetLinkedMovementVector(GameDirection direction)
    {
        return direction;
    }
}