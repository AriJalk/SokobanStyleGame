using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private Transform solvedPanel;
    [SerializeField]
    private Button returnButton;

    private UnityEvent solvedEvent;

    private void OnDestroy()
    {
        solvedEvent?.RemoveListener(Win);
        returnButton.onClick.RemoveAllListeners();
    }

    private void Win()
    {
        solvedPanel.gameObject.SetActive(true);
        returnButton.onClick.AddListener(ReturnToMenu);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Initialize(UnityEvent solvedEvent)
    {
        this.solvedEvent = solvedEvent;
        solvedEvent.AddListener(Win);
    }



}