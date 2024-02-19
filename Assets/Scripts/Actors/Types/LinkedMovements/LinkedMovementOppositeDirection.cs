using UnityEngine;

public class LinkedMovementOppositeDirection : ILinkedMovement
{
    public GameDirection GetLinkedMovementVector(GameDirection direction)
    {
        return GameUtilities.GetOppositeDirection(direction);
    }

}