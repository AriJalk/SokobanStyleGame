using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class ActorManager
{
    public ActorObject[,] ActorsGrid;
    private Dictionary<Type, ActorType> actorTypes;
    private PrefabManager prefabManager;

    public ActorManager(UnityEvent<IActorCommands> inputEvents, short gridSize, PrefabManager prefabManager, Transform parent)
    {
        this.prefabManager = prefabManager;
        ActorsGrid = new ActorObject[gridSize, gridSize];
        actorTypes = new Dictionary<Type, ActorType>();

        PlayableActorType playableType = new PlayableActorType(inputEvents);

        actorTypes.Add(typeof(PlayableActorType), playableType);

        ActorObject actor = new GameObject().AddComponent<ActorObject>();
        ActorsGrid[0, 0] = actor;
        actor.SetActorType(playableType);
        actor.name = playableType.Name + "object";
        actor.transform.SetParent(parent);
        actor.SetGamePosition(new Vector2Int(0, 0));
        actor.transform.localScale = Vector3.one;
        actor.transform.localPosition = new Vector3(actor.GamePosition.x, 0, actor.GamePosition.y);
        GameObject model = prefabManager.RetrievePoolObject("Player");
        GameUtilities.SetParentAndResetPosition(model.transform, ActorsGrid[0, 0].transform);
        
    }
}
