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
        // TODO: Data driven bindings, Input.GetButton support
        BindingDictionary = new Dictionary<GameActions, ActionBinding>()
        {
            {GameActions.MoveUp, new ActionBinding(GameActions.MoveUp, new List<KeyCode> {KeyCode.UpArrow, KeyCode.Keypad8, KeyCode.W}) },
            {GameActions.MoveDown, new ActionBinding(GameActions.MoveDown, new List<KeyCode> {KeyCode.DownArrow, KeyCode.Keypad2, KeyCode.S}) },
            {GameActions.MoveLeft, new ActionBinding(GameActions.MoveLeft, new List<KeyCode> {KeyCode.LeftArrow, KeyCode.Keypad4, KeyCode.A}) },
            {GameActions.MoveRight, new ActionBinding(GameActions.MoveRight, new List<KeyCode> {KeyCode.RightArrow, KeyCode.Keypad6, KeyCode.D}) },
            {GameActions.Undo, new ActionBinding(GameActions.Undo, new List<KeyCode> {KeyCode.Z, KeyCode.U}) },
            {GameActions.Reset, new ActionBinding(GameActions.Reset, KeyCode.R) },
        };
        // Set negatives
        BindingDictionary[GameActions.MoveUp].SetNegativeAction(BindingDictionary[GameActions.MoveDown]);
        BindingDictionary[GameActions.MoveLeft].SetNegativeAction(BindingDictionary[GameActions.MoveRight]);
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

    private void ProccessKeys(List<ActionBinding> bindings, out List<ActionBinding> downTriggers, out List<ActionBinding> holdTriggers)
    {
        downTriggers = new List<ActionBinding>();
        holdTriggers = new List<ActionBinding>();

        foreach (ActionBinding binding in bindings)
        {
            foreach (KeyCode key in binding.Keys)
            {
                //Read key press down, if not check for holding
                if (Input.GetKeyDown(key))
                {
                    downTriggers.Add(binding);
                    break;
                }
                else if (Input.GetKey(key))
                {
                    holdTriggers.Add(binding);
                    break;
                }
            }
        }
    }

    private void TriggerActions(List<ActionBinding> bindings, bool isHold)
    {
        while (bindings.Count > 0)
        {
            ActionBinding binding = bindings[0];
            // Remove pairs of simultanious negative actions
            if (binding.NegativeAction != null && bindings.Contains(binding.NegativeAction))
            {
                bindings.Remove(binding.NegativeAction);
                bindings.Remove(binding);
            }
            else
            {
                if (isHold)
                    OnActionHeld(binding.Action);
                else
                    OnActionDown(binding.Action);

                bindings.Remove(binding);
            }
        }
    }


    public void Listen()
    {
        List<ActionBinding> bindings = new List<ActionBinding>(BindingDictionary.Values);
        List<ActionBinding> downTriggers, holdTriggers;

        ProccessKeys(bindings, out downTriggers, out holdTriggers);

        if (downTriggers.Count > 0)
            TriggerActions(downTriggers, false);
        else if (holdTriggers.Count > 0)
            TriggerActions(holdTriggers, true);

    }
}