using UnityEngine;
using UnityEngine.UI;

namespace SPG.LevelEditor
{
    public class NewFlexibleGrid : MonoBehaviour
    {
        [SerializeField]
        private GridLayoutGroup layoutGroup;
        [SerializeField]
        private RectTransform rectTransform;

        public int GridConstraint
        {
            get
            {
                return layoutGroup.constraintCount;
            }
        }

        private void Awake()
        {
            UpdateRectSize();
            UpdateCellSize();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void UpdateRectSize()
        {
            
            float smallerEdge = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height);

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, smallerEdge);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, smallerEdge);

        }
        private void UpdateCellSize()
        {
            if (rectTransform != null && layoutGroup != null)
            {
                // Calculate the available space for cells (excluding padding and spacing)
                float availableWidth = rectTransform.rect.width - layoutGroup.padding.horizontal - (layoutGroup.spacing.x * (layoutGroup.constraintCount - 1));

                // Calculate the desired cell size based on the available space
                int constraintCount = layoutGroup.constraintCount;
                float cellWidth = availableWidth / constraintCount;

                // Set the cell size
                layoutGroup.cellSize = new Vector2(cellWidth, cellWidth);
            }
        }



    }
}