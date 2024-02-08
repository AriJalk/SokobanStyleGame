using UnityEngine.Events;

public class PlayerType : EntityActorType
{
    public PlayerType() : base(ActorTypeEnum.Player, true, false)
    {
    }
}