using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private Button loadLevelTextButton;
    [SerializeField]
    private Button loadLevel1Button;
    [SerializeField]
    private Button loadLevel2Button;
    [SerializeField]
    private Button loadLevelEditorButton;
    [SerializeField]
    private TMP_InputField inputField;

    private LevelBuilder levelBuilder;
    

    private void Awake()
    {
        levelBuilder = new LevelBuilder();

        loadLevelTextButton.onClick.AddListener(LoadLevelText);
        loadLevel1Button.onClick.AddListener(LoadLevel1);
        loadLevel2Button.onClick.AddListener(LoadLevel2);
        loadLevelEditorButton.onClick.AddListener(LoadLevelEditor);
    }

    private void OnDestroy()
    {
        loadLevelTextButton.onClick.RemoveAllListeners();
        loadLevel1Button.onClick.RemoveAllListeners();
        loadLevel2Button.onClick.RemoveAllListeners();
        loadLevelEditorButton.onClick.RemoveAllListeners();
    }

    private void LoadLevel(LevelStruct level)
    {

        AsyncOperation load = SceneManager.LoadSceneAsync(1);
        load.completed += z => { StaticManager.GameManager.InitializeGameScene(level); };
        

    }

    private void LoadLevel1()
    {
        LoadLevel(levelBuilder.Levels[0]);
    }
    private void LoadLevel2()
    {
        LoadLevel(levelBuilder.Levels[1]);
    }

    private void LoadLevelEditor()
    {
        SceneManager.LoadScene(2);
    }

    private void LoadLevelText()
    {
        if (inputField.text == string.Empty)
            return;
        LevelStruct level = JsonUtility.FromJson<LevelStruct>(inputField.text);
        level.DeserializeFields();
        if(level.EntityGrid != null && level.TileGrid != null)   
            LoadLevel(level);
    }
}