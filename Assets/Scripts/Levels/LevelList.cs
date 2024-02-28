using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Level list of either prebuilt or custom levels
/// </summary>
public class LevelList : MonoBehaviour
{

    UnityEvent<Toggle> toggleChangedEvent;

    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Transform layoutGroup;
    [SerializeField]
    private Button loadButton;
    [SerializeField]
    private Button editButton;
    [SerializeField]
    private Button menuButton;
    [SerializeField]
    private Button deleteButton;

    private string selectedFile = string.Empty;
    private Dictionary<string, GameObject> fileObjectDictionary;


    private void Awake()
    {
        toggleChangedEvent = new UnityEvent<Toggle>();
        toggleChangedEvent.AddListener(OnToggleChanged);

        loadButton.onClick.AddListener(LoadLevel);
        editButton.onClick.AddListener(EditLevel);
        menuButton.onClick.AddListener(ReturnToMenu);
        deleteButton.onClick.AddListener(DeleteLevel);


    }

    // Start is called before the first frame update
    void Start()
    {
        
        fileObjectDictionary = new Dictionary<string, GameObject>();
        ColorBlock block = new ColorBlock();
        block.normalColor = Color.black;
        block.highlightedColor = new Color(238f / 255f, 233f / 255f, 78f / 255f);
        block.selectedColor = new Color(253 / 255f, 243 / 255f, 85 / 255f);
        block.disabledColor = Color.white;
        block.pressedColor = Color.white;
        block.colorMultiplier = 1;
        block.fadeDuration = 0.1f;

        List<string> files;
        switch (StaticManager.GameState)
        {
            case GameState.BuiltIn:
                files = FileManager.GetBuildLevelFiles();
                editButton.gameObject.SetActive(false);
                deleteButton.gameObject.SetActive(false);
                break;
            case GameState.Custom:
                files = FileManager.GetCustomLevelFiles();
                break;
            default:
                files = null;
                break;
        }
        foreach (string file in files)
        {
            GameObject fileObject = new GameObject(file, typeof(RectTransform));
            fileObjectDictionary.Add(file, fileObject );
            //TODO: dynamic cell size
            //fileObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
            GameObject textObject = new GameObject("Text", typeof(TextMeshProUGUI));
            textObject.transform.SetParent(fileObject.transform);
            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();


            text.text = file;
            text.enableAutoSizing = true;
            text.fontSizeMax = 20;
            
            fileObject.transform.SetParent(layoutGroup);


            Toggle toggle = fileObject.AddComponent<Toggle>();
            toggle.group = layoutGroup.GetComponent<ToggleGroup>();
            toggle.targetGraphic = text;
            toggle.colors = block;
            toggle.isOn = false;

            ToggleObject toggleObject = fileObject.AddComponent<ToggleObject>();
            toggleObject.Initialize(toggle, toggleChangedEvent);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        toggleChangedEvent?.RemoveAllListeners();
        loadButton.onClick.RemoveListener(LoadLevel);
        editButton.onClick.RemoveListener(EditLevel);
        menuButton.onClick.RemoveListener(ReturnToMenu);
        deleteButton.onClick.RemoveListener(DeleteLevel);
    }

    private void OnToggleChanged(Toggle toggle)
    {
        selectedFile = toggle.name;
    }


    private void LoadLevel()
    {
        LevelStruct level;
        if (selectedFile != string.Empty)
        {
            switch (StaticManager.GameState)
            {
                case GameState.BuiltIn:
                    level = FileManager.LoadBuildLevel(selectedFile);
                    break;
                case GameState.Custom:
                    level = FileManager.LoadCustomLevel(selectedFile);
                    break;
                default:
                    return;
            }
            if(level.TileGrid != null && level.EntityGrid != null && level.BorderList != null)
            {
                AsyncOperation load = SceneManager.LoadSceneAsync(1);
                load.completed += z => { StaticManager.GameManager.InitializeGameScene(level); };
            }
            
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void EditLevel()
    {
        if(selectedFile != string.Empty)
        {
            StaticManager.LevelName = selectedFile;
            StaticManager.GameState = GameState.Edit;
        }
        SceneManager.LoadScene(2);
    }

    private void DeleteLevel()
    {
        if (selectedFile != string.Empty)
        {
            GameObject levelObject = fileObjectDictionary[selectedFile];
            fileObjectDictionary.Remove(selectedFile);
            Destroy(levelObject);
            FileManager.DeleteCustomLevel(selectedFile);
            selectedFile = string.Empty;

        }
    }

}
