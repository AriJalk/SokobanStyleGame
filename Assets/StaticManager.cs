using UnityEngine;

public static class StaticManager
{
    public static GameManager GameManager;

    static StaticManager()
    {
        GameObject managerObject = new GameObject("GameManager",typeof(GameManager));
        GameManager = managerObject.GetComponent<GameManager>();
    }
}