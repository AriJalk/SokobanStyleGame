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
    
    private int grid_size;
    private float offset;

    public List<ActorObject> PlayableActors {  get; private set; }
    public ActorObject[,] ActorsGrid {  get; private set; }
    public Dictionary<ActorTypeEnum, ActorType> ActorTypes { get; private set; }
    public Dictionary<Color, CubeActorType> CubeColorTypes {  get; private set; }



    public ActorManager(GameManager manager)
    {
        grid_size = GameManager.GRID_SIZE;
        offset = GameManager.OFFSET;
        actorsLayer = manager.ActorLayerTransform;
        this.prefabManager = manager.PrefabManager;
        PlayableActors = new List<ActorObject>();
        ActorsGrid = new ActorObject[grid_size, grid_size];
        ActorTypes = new Dictionary<ActorTypeEnum, ActorType>();
        CubeColorTypes = new Dictionary<Color, CubeActorType>();

        PlayerType playableType = new PlayerType();
        ActorTypes.Add(playableType.TypeName, playableType);

        CubeColorTypes.Add(Color.red, new CubeActorType(GameColors.Red));
        CubeColorTypes.Add(Color.blue, new CubeActorType(GameColors.Blue));

        EntityActorType sphere = new EntityActorType(ActorTypeEnum.Sphere, false, true);
        ActorTypes.Add(sphere.TypeName, sphere);

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

    private void DetachModel(ActorObject actor)
    {
        while(actor.transform.childCount > 0)
        {
            prefabManager.ReturnPoolObject(actor.ActorType.TypeName, actor.transform.GetChild(0).gameObject);
        }
    }

    private void AttachModel(ActorObject actor)
    {
        GameObject model = prefabManager.RetrievePoolObject(actor.ActorType.TypeName);
        GameUtilities.SetParentAndResetPosition(model.transform, actor.transform);
    }

    public ActorObject CreateNewActor(ActorType actorType)
    {
        ActorObject actor = new GameObject().AddComponent<ActorObject>();
        actor.SetActorType(actorType);
        actor.name = actor.ActorType.TypeName + " Object";
        AttachModel(actor);
        return actor;
    }
    public ActorObject CreateNewCube(Color color)
    {
        ActorObject actor = new GameObject().AddComponent<ActorObject>();
        actor.SetActorType(CubeColorTypes[color]);
        actor.name = actor.ActorType.TypeName + " Object";
        AttachModel(actor);
        CubeColorTypes[color].SetCubeColor(actor);
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

    public void ChangeActorType(ActorObject actor, ActorTypeEnum type) 
    {
        DetachModel(actor);
        actor.SetActorType(ActorTypes[type]);
        AttachModel(actor);
    }
}
