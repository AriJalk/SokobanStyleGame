using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        files = FileManager.GetLevelFiles();
        foreach (string file in files)
        {
            GameObject fileObject = new GameObject(file, typeof(RectTransform));
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


            //GameUtilities.SetAnchorsAndResetPosition(textObject.GetComponent<RectTransform>(), Vector2.one * 0.1f, Vector2.one * 0.9f);
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

    void OnToggleChanged(Toggle toggle)
    {
        selectedFile = toggle.name;
    }


    private void LoadLevel()
    {
        if (selectedFile != string.Empty)
        {
            LevelStruct level = FileManager.LoadLevel(selectedFile);

            AsyncOperation load = SceneManager.LoadSceneAsync(1);
            load.completed += z => { StaticManager.GameManager.InitializeGameScene(level); };
        }



    }
}
