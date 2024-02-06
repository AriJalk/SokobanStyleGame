using UnityEngine.Events;

public class PlayableActorType : ActorType
{
    const string NAME = "Playable Actor";

    public PlayableActorType() : base(NAME)
    {
        ResourceName = "Player";
    }
}