using UnityEngine;

/// <summary>
/// Contains objects that need global access
/// </summary>
public static class StaticManager
{
    public static GameManager GameManager;
    public static GameState GameState;
    public static string LevelName;
    static StaticManager()
    {
        GameState = GameState.Menu;
        LevelName = string.Empty;
        GameObject managerObject = new GameObject("GameManager",typeof(GameManager));
        GameManager = managerObject.GetComponent<GameManager>();
    }
}