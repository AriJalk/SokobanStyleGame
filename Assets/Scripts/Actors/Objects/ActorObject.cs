﻿
using UnityEngine;

/// <summary>
/// Represents all game elements in the scene 
/// </summary>
public class ActorObject : MonoBehaviour
{
    public Vector2Int GamePosition { get; private set; }
    public ActorType ActorType { get; protected set; }

    public void SetGamePosition(Vector2Int gamePosition)
    {
        GamePosition = gamePosition;
    }

    public void SetActorType(ActorType actorType)
    {
        ActorType = actorType;
    }
}