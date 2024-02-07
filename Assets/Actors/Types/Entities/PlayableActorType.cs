using UnityEngine.Events;

public class PlayableActorType : EntityActorType
{
    //TODO: general
    const string NAME = "Playable Actor";

    public PlayableActorType() : base(NAME, true)
    {
        ResourceName = "Player";
    }
}