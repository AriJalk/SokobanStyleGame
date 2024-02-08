using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;


public class ActorManager
{
    private PrefabManager prefabManager;
    private Transform actorsLayer;
    private List<ActorObject> playableActors;
    private int grid_size;
    private float offset;

    public ActorObject[,] ActorsGrid {  get; private set; }
    public Dictionary<ActorTypeEnum, ActorType> ActorTypes { get; private set; }



    public ActorManager(GameManager manager)
    {
        grid_size = GameManager.GRID_SIZE;
        offset = GameManager.OFFSET;
        actorsLayer = manager.ActorLayerTransform;
        this.prefabManager = manager.PrefabManager;
        ActorsGrid = new ActorObject[grid_size, grid_size];
        ActorTypes = new Dictionary<ActorTypeEnum, ActorType>();

        PlayerType playableType = new PlayerType();
        ActorTypes.Add(playableType.TypeName, playableType);

        EntityActorType cube = new EntityActorType(ActorTypeEnum.Cube, true);
        ActorTypes.Add(cube.TypeName, cube);
        EntityActorType sphere = new EntityActorType(ActorTypeEnum.Sphere, false);
        ActorTypes.Add(sphere.TypeName, sphere);

        TileType tileType = new TileType(ActorTypeEnum.BasicTile);
        ActorTypes.Add(tileType.TypeName, tileType);

        GoalTileType goalTileType = new GoalTileType(GameColors.Red);
        ActorTypes.Add(goalTileType.TypeName, goalTileType);
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
        actor.name = actor.ActorType.TypeName + " Object";
        //Attach type model
        GameObject model = prefabManager.RetrievePoolObject(actorType.TypeName);
        GameUtilities.SetParentAndResetPosition(model.transform, actor.transform);
        return actor;
    }

    public void AddActorToTile(ActorObject actor, ActorObject tile)
    {
        TileType tileType = tile.ActorType as TileType;
        if (tileType != null)
        {
            int posX = tile.GamePosition.x;
            int posY = tile.GamePosition.y;
            if (ActorsGrid[tile.GamePosition.x, tile.GamePosition.y] == null)
            {
                actor.transform.SetParent(actorsLayer);
                actor.SetGamePosition(tile.GamePosition);
                actor.transform.localScale = Vector3.one;
                actor.transform.localPosition = CalculateLocalPosition(actor);
                ActorsGrid[tile.GamePosition.x, tile.GamePosition.y] = actor;
            }
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
