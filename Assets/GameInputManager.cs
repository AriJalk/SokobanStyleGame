using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameInputManager
{
    public UnityEvent<GameActions> ActionTriggeredEvent;

    public const float INPUT_FREQUENCY = 0.1f;
    public const float INPUT_REPEAT_DELAY = 0.2f;
    public const float REPEAT_ACCELERATION = 5000f;
    public const short REPEAT_LIMIT = 10;


    // Fields used for action button pressed over time
    private GameActions lastGameAction;
    private float inputHoldTimer;
    private float inputRepeatDelayTimer;
    private int repeats = 0;


    public Dictionary<GameActions, ActionBinding> BindingDictionary { get; private set; }


    public GameInputManager()
    {
        ActionTriggeredEvent = new UnityEvent<GameActions>();
        // Create all action bindings
        // TODO: Data driven bindings
        BindingDictionary = new Dictionary<GameActions, ActionBinding>()
        {
            {GameActions.MoveUp, new ActionBinding(GameActions.MoveUp, new List<KeyCode> {KeyCode.UpArrow, KeyCode.Keypad8, KeyCode.W}) },
            {GameActions.MoveDown, new ActionBinding(GameActions.MoveDown, new List<KeyCode> {KeyCode.DownArrow, KeyCode.Keypad2, KeyCode.S}) },
            {GameActions.MoveLeft, new ActionBinding(GameActions.MoveLeft, new List<KeyCode> {KeyCode.LeftArrow, KeyCode.Keypad4, KeyCode.A}) },
            {GameActions.MoveRight, new ActionBinding(GameActions.MoveRight, new List<KeyCode> {KeyCode.RightArrow, KeyCode.Keypad6, KeyCode.D}) },
            {GameActions.Undo, new ActionBinding(GameActions.Undo, new List<KeyCode> {KeyCode.Z, KeyCode.U}) },
        };
    }

    private void OnActionDown(GameActions action)
    {
        // Set action as current and reset timer
        lastGameAction = action;
        inputHoldTimer = 0f;
        inputRepeatDelayTimer = 0f;
        repeats = 0;
        ActionTriggeredEvent?.Invoke(action);
    }
    private void OnActionHeld(GameActions action)
    {
        if (lastGameAction == action)
        {
            // Add to delay timer between frequency triggers if threshold not reached
            if (inputRepeatDelayTimer < INPUT_REPEAT_DELAY)
            {
                inputRepeatDelayTimer += Time.deltaTime;
            }
            // Add to hold timer if threshold met
            else
            {
                // accelerate movement frequency as time goes on with action held
                inputHoldTimer += Time.deltaTime + repeats / REPEAT_ACCELERATION;
            }
            // Once threshold reached reached call event
            if (inputHoldTimer > INPUT_FREQUENCY)
            {
                inputHoldTimer = 0f;
                ActionTriggeredEvent?.Invoke(action);
                if (repeats < REPEAT_LIMIT)
                    repeats++;
            }
        }
    }

    public void Listen()
    {
        foreach (ActionBinding binding in BindingDictionary.Values)
        {
            foreach (KeyCode key in binding.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    OnActionDown(binding.Action);
                    break;
                }
                else if (Input.GetKey(key))
                {
                    OnActionHeld(binding.Action);
                    break;
                }
            }
        }
    }
}