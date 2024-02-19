using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUserInterface : MonoBehaviour
{
    [SerializeField]
    private Transform solvedPanel;
    [SerializeField]
    private Button returnButton;
    [SerializeField]
    private Button menuButton;

    private UnityEvent solvedEvent;

    private void Start()
    {
        menuButton.onClick.AddListener(ReturnToMenu);
    }
    private void OnDestroy()
    {
        solvedEvent?.RemoveListener(Win);
        returnButton.onClick.RemoveListener(ReturnToMenu);
        menuButton.onClick.RemoveListener(ReturnToMenu);
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