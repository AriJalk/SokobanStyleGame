using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Binding container that correlates raw input and game actions
/// </summary>
public struct ActionBinding
{
    public List<KeyCode> Keys { get; private set; }
    public GameActions Action { get; private set; }

    public ActionBinding(GameActions action)
    {
        Keys = new List<KeyCode>();
        Action = action;
    }

    public ActionBinding(GameActions action, List<KeyCode> keys)
    {
        Keys = keys;
        Action = action;
    }

    public ActionBinding(GameActions action, KeyCode key)
    {
        Keys = new List<KeyCode>();
        Action = action;
        Keys.Add(key);
    }
}