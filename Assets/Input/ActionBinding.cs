using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Binding container that correlates raw input and game actions
/// </summary>
public class ActionBinding
{
    public List<KeyCode> Keys { get; private set; }
    public GameActions Action { get; private set; }

    private ActionBinding _negativeAction;
    public ActionBinding NegativeAction
    {
        get
        {
            return _negativeAction;
        }
        private set
        {
            if(value != null && value != this)
            {
                _negativeAction = value;
            }
        }
    }

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

    public void SetNegativeAction(ActionBinding negative)
    {
        NegativeAction = negative;
        negative.NegativeAction = this;
    }
}