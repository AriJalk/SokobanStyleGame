using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class ActorManager
{
    private UnityEvent<IActorCommands> inputEvents;


    public ActorObject[,] ActorsGrid;
    public Dictionary<string, ActorType> ActorTypes;
    private PrefabManager prefabManager;
    private Transform actorsLayer;
    private List<ActorObject> playableActors;
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
        inputEvents = manager.InputEvents;
        PlayableActorType playableType = new PlayableActorType();
        ActorTypes.Add("Player", playableType);

        ActorType cube = new ActorType("Cube");
        cube.SetResourceName("Cube");
        cube.SetCanPush(true);
        ActorTypes.Add("Cube", cube);
        ActorType sphere = new ActorType("Sphere");
        sphere.SetResourceName("Sphere");
        ActorTypes.Add("Sphere", sphere);
    }

    private Vector3 CalculateLocalPosition(ActorObject actor)
    {
        Vector3 position = new Vector3();
        int posX = actor.GamePosition.x;
        int posY = actor.GamePosition.y;
        if (actor != null)
        {
            position.x = posX + posX * offset;
            position.z = posY + posY * offset;
        }
        return position;
    }

    public ActorObject CreateNewActor(ActorType actorType)
    {
        ActorObject actor = new GameObject().AddComponent<ActorObject>();
        actor.SetActorType(actorType);
        actor.name = actor.ActorType.Name + " Object";
        //Attach type model
        GameObject model = prefabManager.RetrievePoolObject(actorType.ResourceName);
        GameUtilities.SetParentAndResetPosition(model.transform, actor.transform);
        return actor;
    }

    public void AddActorToTile(ActorObject actor, TileObject tile)
    {
        int posX = tile.GridPosition.x;
        int posY = tile.GridPosition.y;
        if (ActorsGrid[tile.GridPosition.x, tile.GridPosition.y] == null)
        {
            actor.transform.SetParent(actorsLayer);
            actor.SetGamePosition(tile.GridPosition);
            actor.transform.localScale = Vector3.one;
            actor.transform.localPosition = CalculateLocalPosition(actor);
            ActorsGrid[tile.GridPosition.x, tile.GridPosition.y] = actor;
        }
    }

    public ActorObject GetActor(Vector2Int position)
    {
        if (GameUtilities.IsPositionInBounds(ActorsGrid, position))
            return ActorsGrid[position.x, position.y];
        return null;
    }

    public void MoveActor(ActorObject actor, Vector2Int directionVector)
    {
        ActorsGrid[actor.GamePosition.x, actor.GamePosition.y] = null;
        Vector2Int newPosition = actor.GamePosition + directionVector;
        ActorsGrid[newPosition.x, newPosition.y] = actor;
        actor.SetGamePosition(newPosition);
        actor.transform.localPosition = CalculateLocalPosition(actor);
    }
}
