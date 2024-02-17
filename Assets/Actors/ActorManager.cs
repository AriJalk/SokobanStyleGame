using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles actor functionality (game objects on the map)
/// </summary>
public class ActorManager
{
    private PrefabManager prefabManager;
    private Transform actorsLayer;
    
    private int grid_size;
    private float offset;

    /// <summary>
    /// Contains actors which respond to player input
    /// </summary>
    public List<ActorObject> PlayableActors {  get; private set; }
    /// <summary>
    /// Placement of each actor on the grid
    /// </summary>
    public ActorObject[,] ActorsGrid {  get; private set; }
    /// <summary>
    /// Shared actor types
    /// </summary>
    public Dictionary<ActorTypeEnum, ActorType> ActorTypes { get; private set; }
    /// <summary>
    /// Shared cube color types
    /// </summary>
    public Dictionary<Color, CubeActorType> CubeColorTypes {  get; private set; }



    public ActorManager(GameManager manager)
    {
        grid_size = manager.Grid_Size;
        offset = GameManager.OFFSET;
        actorsLayer = manager.ActorLayerTransform;
        prefabManager = manager.PrefabManager;
        PlayableActors = new List<ActorObject>();
        ActorsGrid = new ActorObject[grid_size, grid_size];
        ActorTypes = new Dictionary<ActorTypeEnum, ActorType>();
        CubeColorTypes = new Dictionary<Color, CubeActorType>();

        // Add player
        PlayerType playableType = new PlayerType();
        ActorTypes.Add(playableType.TypeName, playableType);


        // Add cubes
        CubeColorTypes.Add(Color.red, new CubeActorType(GameColors.Red, new LinkedMovementOppositeDirection()));
        CubeColorTypes.Add(Color.blue, new CubeActorType(GameColors.Blue, new LinkedMovementSameDirection()));

        EntityActorType.SetLinkedTypes(CubeColorTypes[Color.red], CubeColorTypes[Color.blue]);

        // Add sphere
        EntityActorType sphere = new EntityActorType(ActorTypeEnum.Sphere, false, true);
        ActorTypes.Add(sphere.TypeName, sphere);

    }

    // Convert actor local position to world Position
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

    // Detach the model gameObject from an actor
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

    public T CreateNewActor<T>(ActorType actorType) where T : ActorObject
    {
        T actor = new GameObject().AddComponent<T>();
        actor.SetActorType(actorType);
        actor.name = actor.ActorType.TypeName + " Object";
        AttachModel(actor);
        actorType.ActorObjectList.Add(actor);
        return actor;
    }

    public ActorObject CreateNewCube(Color color)
    {
        ActorObject actor = CreateNewActor<ActorObject>(CubeColorTypes[color]);
        CubeColorTypes[color].SetCubeColor(actor);
        return actor;
    }


    public void AddActorToTile(ActorObject actor, TileObject tile)
    {
        if (tile != null)
        {
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


    public T GetActorType<T>(ActorObject actor) where T : ActorType
    {
        if(actor.ActorType is T type)
        {
            return type;
        }
        return null;
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
