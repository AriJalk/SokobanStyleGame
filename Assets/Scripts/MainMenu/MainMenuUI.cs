using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private Button loadBuiltInLevelButton;
    [SerializeField]
    private Button loadCustomLevelButtom;
    [SerializeField]
    private Button loadLevelEditorButton;

    private LevelBuilder levelBuilder;
    

    private void Awake()
    {
        levelBuilder = new LevelBuilder();

        loadBuiltInLevelButton.onClick.AddListener(LoadBuiltInLevelList);
        loadCustomLevelButtom.onClick.AddListener(LoadCustomLevelList);
        loadLevelEditorButton.onClick.AddListener(LoadLevelEditor);

        StaticManager.GameState = GameState.Menu;
    }

    private void OnDestroy()
    {
        loadBuiltInLevelButton.onClick.RemoveListener(LoadBuiltInLevelList);
        loadCustomLevelButtom.onClick.RemoveListener(LoadCustomLevelList);
        loadLevelEditorButton.onClick.RemoveListener(LoadLevelEditor);
    }

    private void LoadLevel(LevelStruct level)
    {

        AsyncOperation load = SceneManager.LoadSceneAsync(1);
        load.completed += z => { StaticManager.GameManager.InitializeGameScene(level); };
        

    }

    private void LoadBuiltInLevelList()
    {
        StaticManager.GameState = GameState.BuiltIn;
        AsyncOperation load = SceneManager.LoadSceneAsync(3); 
    }

    private void LoadCustomLevelList()
    {
        StaticManager.GameState = GameState.Custom;
        AsyncOperation load = SceneManager.LoadSceneAsync(3);
    }

    private void LoadLevelEditor()
    {
        SceneManager.LoadScene(2);
    }

    /*private void LoadLevelText()
    {
        if (inputField.text == string.Empty)
            return;
        LevelStruct level = JsonUtility.FromJson<LevelStruct>(inputField.text);
        level.DeserializeFields();
        if(level.EntityGrid != null && level.TileGrid != null)   
            LoadLevel(level);
    }*/
}