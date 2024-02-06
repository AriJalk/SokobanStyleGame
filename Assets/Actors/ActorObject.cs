
using UnityEngine;

public class ActorObject : MonoBehaviour
{
    public Vector2Int GamePosition { get; private set; }
    public ActorType ActorType { get; protected set; }

    public void SetGamePosition(Vector2Int gamePosition)
    {
        this.GamePosition = gamePosition;
    }

    public void SetActorType(ActorType actorType)
    {
        this.ActorType = actorType;
    }
}