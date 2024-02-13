using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SPG.LevelEditor
{
    public class LevelEditorUI : MonoBehaviour
    {
        public UnityEvent<GridCellObject> CellClickedEvent;
        [SerializeField]
        private FlexibleGrid LevelGrid;

        private Vector2Int lastPosition;

        private void Awake()
        {
            BuildGrid();
            CellClickedEvent = new UnityEvent<GridCellObject>();
            //Set to number outside of grid
            lastPosition = new Vector2Int(-99, -99);
        }

        private void Start()
        {

        }

        private void Update()
        {
            if(Input.GetMouseButton(0))
            {

                Transform cell = UIRaycaster.Raycast(Input.mousePosition, LayerMask.GetMask("GridCell"));
                
                if (cell != null && cell.GetComponent<GridCellObject>() is GridCellObject cellObject && cellObject.GamePosition != lastPosition)
                {
                    lastPosition = cellObject.GamePosition;
                    Debug.Log(cellObject.GamePosition);
                    CellClickedEvent?.Invoke(cellObject);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                lastPosition = new Vector2Int(-99,-99);
            }
        }

        private void BuildGrid()
        {
            for(int i = 0; i < LevelGrid.GridConstraint; i++)
            {
                for (int j = 0; j < LevelGrid.GridConstraint; j++)
                {
                    GameObject cell = new GameObject($"Cell_[{i},{j}]", typeof(RectTransform));
                    cell.layer = LayerMask.NameToLayer("GridCell");
                    Image image = cell.AddComponent<Image>();
                    image.sprite = Resources.Load<Sprite>("Empty");
                    image.color = Color.gray;

                    GridCellObject cellObject = cell.AddComponent<GridCellObject>();
                    cellObject.GamePosition = new Vector2Int(i, j);
                    GameUtilities.SetParentAndResetPosition(cell.transform, LevelGrid.transform);

                    GameObject entityOnCell = new GameObject("Text", typeof(TextMeshProUGUI));

                    GameUtilities.SetParentAndResetPosition(entityOnCell.transform, cell.transform);
                    GameUtilities.ResetAnchors(entityOnCell.GetComponent<RectTransform>());
                    cellObject.EntityOnTile = entityOnCell.GetComponent<TextMeshProUGUI>();
                    
                }
            }
        }

        private void TestLayer(LayerMask layerMask)
        {
            Debug.Log("LayerMask binary: " + Convert.ToString(layerMask.value, 2));
        }
    }
}