using UnityEngine;
using UnityEngine.UI;

namespace SPG.LevelEditor
{
    public abstract class LevelEditorCellBase : MonoBehaviour
    {
        public Image Image { get; set; }
        public RectTransform RectTransform { get; set; }
    }
}