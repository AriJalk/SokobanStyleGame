using System;
using UnityEngine;

namespace SPG.LevelEditor
{
    [Serializable]
    public struct LevelEditorBorderStruct
    {
        [SerializeField]
        public Vector2Int PositionA;
        [SerializeField]
        public Vector2Int PositionB;

        public LevelEditorBorderStruct(Vector2Int positionA, Vector2Int positionB)
        {
            PositionA = positionA;
            PositionB = positionB;
        }

        /*public override int GetHashCode()
        {
            return HashCode.Combine(PositionA, PositionB);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }*/
    }
}