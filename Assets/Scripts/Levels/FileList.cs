using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LevelMode
{
    BuiltIn,
    Custom,
}
public class FileList : MonoBehaviour
{

    UnityEvent<Toggle> toggleChangedEvent;

    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Transform layoutGroup;
    [SerializeField]
    private Button loadButton;

    private string selectedFile = string.Empty;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        toggleChangedEvent = new UnityEvent<Toggle>();
        toggleChangedEvent.AddListener(OnToggleChanged);

        loadButton.onClick.AddListener(LoadLevel);

        ColorBlock block = new ColorBlock();
        block.normalColor = Color.black;
        block.highlightedColor = new Color(238f / 255f, 233f / 255f, 78f / 255f);
        block.selectedColor = new Color(253 / 255f, 243 / 255f, 85 / 255f);
        block.disabledColor = Color.white;
        block.pressedColor = Color.white;
        block.colorMultiplier = 1;
        block.fadeDuration = 0.1f;

        List<string> files;
        switch (StaticManager.LevelMode)
        {
            case LevelMode.BuiltIn:
                files = FileManager.GetBuildLevelFiles();
                break;
            case LevelMode.Custom:
                files = FileManager.GetCustomLevelFiles();
                break;
            default:
                files = null;
                break;
        }
        foreach (string file in files)
        {
            GameObject fileObject = new GameObject(file, typeof(RectTransform));
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
            switch (StaticManager.LevelMode)
            {
                case LevelMode.BuiltIn:
                    level = FileManager.LoadBuildLevel(selectedFile);
                    break;
                case LevelMode.Custom:
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

}
