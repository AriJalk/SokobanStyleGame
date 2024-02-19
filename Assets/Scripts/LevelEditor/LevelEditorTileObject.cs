
using TMPro;
using UnityEngine;


namespace SPG.LevelEditor
{
    public class LevelEditorTileObject : LevelEditorCellBase
    {
        public Vector2Int GamePosition { get; set; }
        public TextMeshProUGUI EntityOnTile { get; set; }
    }
}