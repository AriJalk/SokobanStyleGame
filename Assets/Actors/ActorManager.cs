using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class ActorManager
{



    public ActorObject[,] ActorsGrid;
    public Dictionary<string, ActorType> ActorTypes;
    private PrefabManager prefabManager;
    private Transform actorsLayer;
    private int grid_size;
    private float offset;

    public ActorManager(Manager manager)
    {
        grid_size = Manager.GRID_SIZE;
        offset = Manager.OFFSET;
        actorsLayer = manager.ActorLayerTransform;
        this.prefabManager = manager.PrefabManager;
        ActorsGrid = new ActorObject[grid_size, grid_size];
        ActorTypes = new Dictionary<string, ActorType>();

        PlayableActorType playableType = new PlayableActorType(manager.InputEvents);
        ActorTypes.Add("Player", playableType);

        ActorType cube = new ActorType("Cube");
        cube.SetResourceName("Cube");
        ActorTypes.Add("Cube", cube);
        ActorType sphere = new ActorType("Sphere");
        sphere.SetResourceName("Sphere");
        ActorTypes.Add("Sphere", sphere);
    }

    public ActorObject CreateNewActor(ActorType actorType)
    {
        ActorObject actor = new GameObject().AddComponent<ActorObject>();
        actor.SetActorType(actorType);
        //Attach type model
        GameObject model = prefabManager.RetrievePoolObject(actorType.ResourceName);
        GameUtilities.SetParentAndResetPosition(model.transform, actor.transform);
        return actor;
    }

    public void AddActorToTile(ActorObject actor, TileObject tile)
    {
        int posX = tile.GridPosition.x;
        int posY = tile.GridPosition.y;
        if (tile.ObjectSlot == null)
        {
            actor.transform.SetParent(actorsLayer);
            actor.SetGamePosition(tile.GridPosition);
            actor.transform.localScale = Vector3.one;
            actor.transform.localPosition = new Vector3(posX + posX * offset, 0, posY + posY * offset);
            tile.ObjectSlot = actor.gameObject;
        }
    }
}
