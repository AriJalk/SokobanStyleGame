using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SPG.LevelEditor
{
    public enum ButtonEvents
    {
        Save,
        Exit,
    }
    public class LevelEditorUI : MonoBehaviour
    {
        public const float BORDER_SCALE = 4f;
        public const float TILE_GAP = 2f;
        public UnityEvent<GridCellObject> CellClickedEvent;
        public UnityEvent<ButtonEvents> ButtonEvent;
        [SerializeField]
        private VerticalLayoutGroup rowsLayoutGroup;
        [SerializeField]
        private RectTransform gridTransform;
        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button exitButton;

        private Vector2Int lastPosition;
        private Button clickedButton;

        private void Awake()
        {
            //BuildGrid();
            BuildGridNew();
            CellClickedEvent = new UnityEvent<GridCellObject>();
            ButtonEvent = new UnityEvent<ButtonEvents>();
            //Set to number outside of grid
            lastPosition = new Vector2Int(-99, -99);
            saveButton.onClick.AddListener(SaveClicked);
            exitButton.onClick.AddListener(ExitClicked);
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
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
                lastPosition = new Vector2Int(-99, -99);
            }
        }

        private void OnDestroy()
        {
            saveButton.onClick.RemoveListener(SaveClicked);
            exitButton.onClick.RemoveListener(ExitClicked);
        }


        private void BuildGridNew()
        {

            GameUtilities.SquareAnchors(gridTransform, gridTransform.parent);
            //UpdateRectSize(gridTransform);
            gridTransform.sizeDelta = Vector2.zero;
            //rowsLayoutGroup.GetComponent<RectTransform>().sizeDelta = Vector2.zero;


            float cellSize = CalculateCellSize();

            for (int i = 0; i < LevelEditor.GRID_SIZE; i++)
            {
                GameObject row = CreateRowObject(i, cellSize);
                for (int j = 0; j < LevelEditor.GRID_SIZE; j++)
                {
                    // Create Cell
                    GameObject cellObject = CreateCellObject(j, i, row.transform, cellSize);

                    // CreateBorder
                    if (j < LevelEditor.GRID_SIZE - 1)
                    {
                        GameObject borderObject = new GameObject($"Border_[{j},{i}]-[{j + 1},{i}]", typeof(RectTransform));
                        RectTransform borderRect = borderObject.GetComponent<RectTransform>();
                        GameUtilities.SetParentAndResetPosition(borderObject.transform, row.transform);
                        AddEmptyImageComponent(borderObject);
                        borderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize / BORDER_SCALE);
                    }
                }
                if (i < LevelEditor.GRID_SIZE - 1)
                {
                    GameObject borderRow = CreateBorderRow(i, cellSize);
                }
            }
        }

        private GameObject CreateBorderRow(int rowIndex, float cellSize)
        {
            GameObject borderRow = CreateRowObject(rowIndex, cellSize / BORDER_SCALE);
            GameUtilities.SetParentAndResetPosition(borderRow.transform, rowsLayoutGroup.transform);

            for (int i = 0; i < LevelEditor.GRID_SIZE; i++)
            {
                CreateBorder(new Vector2Int(i, rowIndex), new Vector2Int(i, rowIndex + 1), cellSize, borderRow.transform);
                if (i < LevelEditor.GRID_SIZE - 1)
                {
                    GameObject gapObject = new GameObject("Gap", typeof(RectTransform));
                    RectTransform gapRect = gapObject.GetComponent<RectTransform>();
                    GameUtilities.SetParentAndResetPosition(gapObject.transform, borderRow.transform);
                    gapRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize / BORDER_SCALE);
                }
            }


            borderRow.GetComponent<HorizontalLayoutGroup>().spacing = TILE_GAP;
            return borderRow;
        }

        private GameObject CreateBorder(Vector2Int positionA, Vector2Int positionB, float cellSize, Transform parent)
        {
            GameObject borderObject = new GameObject($"Border_[{positionA.x},{positionA.y}]-[{positionB.x},{positionB.y}]", typeof(RectTransform));
            RectTransform borderRect = borderObject.GetComponent<RectTransform>();
            GameUtilities.SetParentAndResetPosition(borderObject.transform, parent);
            AddEmptyImageComponent(borderObject);
            borderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);

            return borderObject;
        }

        private Image AddEmptyImageComponent(GameObject gameObject)
        {
            Image image = gameObject.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("Empty");
            image.color = Color.gray;
            return image;
        }



        private GameObject CreateRowObject(int index, float cellSize)
        {
            GameObject row = new GameObject($"Row_{index}", typeof(RectTransform));
            RectTransform rowRect = row.GetComponent<RectTransform>();
            GameUtilities.SetParentAndResetPosition(row.transform, rowsLayoutGroup.transform);
            rowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize);
            HorizontalLayoutGroup horizontalLayoutGroup = row.AddComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.spacing = TILE_GAP;
            horizontalLayoutGroup.childControlWidth = false;
            horizontalLayoutGroup.childForceExpandWidth = false;
            return row;
        }

        private GameObject CreateCellObject(int x, int y, Transform parent, float cellSize)
        {
            GameObject cell = new GameObject($"Cell_[{x},{y}]", typeof(RectTransform));
            cell.layer = LayerMask.NameToLayer("GridCell");
            Image image = cell.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("Empty");
            image.color = Color.gray;

            GridCellObject cellObject = cell.AddComponent<GridCellObject>();
            // Transform so that the result matrix is rotated 90 degress in data to corespond to X,Y
            cellObject.GamePosition = new Vector2Int(x, LevelEditor.GRID_SIZE - y - 1);
            GameUtilities.SetParentAndResetPosition(cell.transform, parent);

            GameObject entityOnCell = new GameObject("Text", typeof(TextMeshProUGUI));

            GameUtilities.SetParentAndResetPosition(entityOnCell.transform, cell.transform);
            GameUtilities.ResetAnchors(entityOnCell.GetComponent<RectTransform>());
            cellObject.EntityOnTile = entityOnCell.GetComponent<TextMeshProUGUI>();
            cell.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);
            return cell;
        }

        private float CalculateCellSize()
        {
            if (rowsLayoutGroup.GetComponent<RectTransform>() is RectTransform rect)
            {
                float cellCount = LevelEditor.GRID_SIZE;
                float borderCount = cellCount - 1;

                float cells = cellCount + borderCount / BORDER_SCALE;

                float availableWidth = (rect.rect.width - (TILE_GAP * (cellCount + borderCount - 1))) / cells;

                //Debug.Log(availableWidth);
                //Debug.Log(rect.rect.width);
                return availableWidth;
            }
            return 0;
        }




        private void TestLayer(LayerMask layerMask)
        {
            Debug.Log("LayerMask binary: " + Convert.ToString(layerMask.value, 2));
        }

        private void SaveClicked()
        {
            ButtonEvent?.Invoke(ButtonEvents.Save);
        }

        private void ExitClicked()
        {
            ButtonEvent?.Invoke(ButtonEvents.Exit);
        }

        private void UpdateRectSize(RectTransform rectTransform)
        {

            float smallerEdge = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height);

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, smallerEdge);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, smallerEdge);

        }

    }
}