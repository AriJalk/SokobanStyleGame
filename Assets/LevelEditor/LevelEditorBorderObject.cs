using System;
using UnityEngine;

namespace SPG.LevelEditor
{
    public class LevelEditorBorderObject : LevelEditorCellBase
    {
        public Tuple<Vector2Int, Vector2Int> BorderBetweenTiles { get; set; }
    }
}