using UnityEngine;

public class CubeActorType : EntityActorType
{
    public GameColors Color { get; private set; }
    public CubeActorType(GameColors color) : base(ActorTypeEnum.Cube, true)
    {
        Color = color;
    }

    public void SetCubeColor(ActorObject actor)
    {
        MeshRenderer meshRenderer = actor.transform.GetChild(0).Find("Model").GetComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load<Material>(Color + "Material");
    }
}