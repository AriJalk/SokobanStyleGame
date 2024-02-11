using UnityEngine;
using UnityEngine.Events;

public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private Transform WinPanel;

    private UnityEvent solvedEvent;

    private void OnDestroy()
    {
        solvedEvent?.RemoveListener(Win);
    }

    private void Win()
    {
        WinPanel.gameObject.SetActive(true);
    }

    public void Initialize(UnityEvent solvedEvent)
    {
        this.solvedEvent = solvedEvent;
        solvedEvent.AddListener(Win);
    }

}