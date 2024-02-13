using UnityEngine;
using UnityEngine.UI;

namespace SPG.LevelEditor
{
    public class LevelEditorUI : MonoBehaviour
    {
        [SerializeField]
        private FlexibleGrid levelGrid;
        [SerializeField]
        private CameraRaycaster raycaster;


        private void Start()
        {
            BuildGrid();
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = RectTransformUtility.ScreenPointToRay(raycaster.Camera, Input.mousePosition);
                Transform cell = raycaster.Raycast(Input.mousePosition, LayerMask.NameToLayer("GridCell"));
                if (cell != null)
                {
                    Debug.Log(cell.GetComponent<GridCellObject>().GamePosition);
                }

            }
        }

        private void BuildGrid()
        {
            for(int i = 0; i < levelGrid.GridConstraint; i++)
            {
                for (int j = 0; j < levelGrid.GridConstraint; j++)
                {
                    GameObject cell = new GameObject($"Cell_[{i},{j}]", typeof(RectTransform));
                    cell.layer = LayerMask.NameToLayer("GridCell");
                    Image image = cell.AddComponent<Image>();
                    image.sprite = Resources.Load<Sprite>("Empty");
                    image.color = Color.gray;
                    cell.transform.SetParent(levelGrid.transform);
                    GridCellObject cellObject = cell.AddComponent<GridCellObject>();
                    cellObject.GamePosition = new Vector2Int(i, j);
                    
                }
            }
        }
    }
}