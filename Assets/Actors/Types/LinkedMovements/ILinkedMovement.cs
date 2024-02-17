using UnityEngine;

public interface ILinkedMovement
{
    public GameDirection GetLinkedMovementVector(GameDirection direction);
}