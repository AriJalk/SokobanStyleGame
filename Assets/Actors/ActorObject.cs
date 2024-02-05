using UnityEngine;

public class ActorObject : MonoBehaviour
{
    public Vector2Int GamePosition { get; private set; }
    public float GameRotationDegrees { get; private set; }
    public ActorType ActorType { get; private set; }

    public void SetGamePosition(Vector2Int gamePosition)
    {
        this.GamePosition = gamePosition;
    }

    public void SetGameRotation(GameRotation gameRotation)
    {
        float toRotate = 0f;
        switch (gameRotation)
        {
            case GameRotation.Clockwise:
                toRotate = 45f;
                break;
            case GameRotation.CounterClockwise:
                toRotate = -45f;
                break;
            default:
                break;
        }
        GameRotationDegrees += toRotate;
        if(GameRotationDegrees > 360f)
        {
            GameRotationDegrees -= 360f;
        }
        else if(GameRotationDegrees < 0f)
        {
            GameRotationDegrees += 360f;
        }
        transform.Rotate(Vector3.up, toRotate);
    }

    public void SetActorType(ActorType actorType)
    {
        this.ActorType = actorType;
    }
}