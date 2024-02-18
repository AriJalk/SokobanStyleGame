using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FileList : MonoBehaviour
{
    [SerializeField]
    Transform layoutGroup;
    // Start is called before the first frame update
    void Start()
    {
        List<string> files;
        files = FileManager.GetLevelFiles();
        foreach (string file in files)
        {
            GameObject fileObject = new GameObject(file, typeof(RectTransform));
            TextMeshProUGUI text = fileObject.AddComponent<TextMeshProUGUI>();
            text.text = file;
            text.enableAutoSizing = true;
            text.fontSizeMax = 20;
            fileObject.transform.SetParent(layoutGroup);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
