﻿using System;
using System.Collections.Generic;
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
        public UnityEvent<LevelEditorCellBase> CellClickedEvent;
        public UnityEvent<ButtonEvents> ButtonEvent;
        public TMP_InputField InputField;
        [SerializeField]
        private VerticalLayoutGroup rowsLayoutGroup;
        [SerializeField]
        private RectTransform gridTransform;
        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button exitButton;

        private LevelEditorCellBase lastPaintedCell;

        public LevelEditorTileObject[,] TileObjectGrid;
        public Dictionary<LevelEditorBorderStruct, LevelEditorBorderObject> BorderDictionary;
        

        private void Awake()
        {
            //BuildGrid();
            //BuildGridNew();
            CellClickedEvent = new UnityEvent<LevelEditorCellBase>();
            ButtonEvent = new UnityEvent<ButtonEvents>();
            //Set to number outside of grid
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
                //Paint with selected tile / entity
                Transform cell = UIRaycaster.Raycast(Input.mousePosition, LayerMask.GetMask("GridCell"));
                if (cell != null)
                {
                    ProcessCell(cell);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                lastPaintedCell = null;
            }
        }

        private void OnDestroy()
        {
            saveButton.onClick.RemoveListener(SaveClicked);
            exitButton.onClick.RemoveListener(ExitClicked);
        }


        public void BuildGrid()
        {
            BorderDictionary = new Dictionary<LevelEditorBorderStruct, LevelEditorBorderObject>();
            TileObjectGrid = new LevelEditorTileObject[LevelEditor.GRID_SIZE,LevelEditor.GRID_SIZE];
            GameUtilities.SquareAnchors(gridTransform, gridTransform.parent);
            //UpdateRectSize(gridTransform);
            gridTransform.sizeDelta = Vector2.zero;
            //rowsLayoutGroup.GetComponent<RectTransform>().sizeDelta = Vector2.zero;


            float cellSize = CalculateCellSize();

            // Build grid
            for (int i = LevelEditor.GRID_SIZE - 1; i >= 0; i--)
            {

                if (i < LevelEditor.GRID_SIZE - 1)
                {
                    GameObject borderRow = CreateBorderRow(i, cellSize);
                }

                GameObject row = CreateRowObject(i, cellSize);
                for (int j = 0; j < LevelEditor.GRID_SIZE; j++)
                {
                    // Create Cell
                    GameObject cellObject = CreateCellObject(j, i, row.transform, cellSize);
                    TileObjectGrid[j, i] = cellObject.GetComponent<LevelEditorTileObject>();
                    // CreateBorder
                    if (j < LevelEditor.GRID_SIZE - 1)
                    {
                        Tuple<Vector2Int, Vector2Int> borderBetween = new Tuple<Vector2Int, Vector2Int>(new Vector2Int(j, i), new Vector2Int(j + 1, i));
                        LevelEditorBorderObject borderObject = CreateBorder(borderBetween.Item1, borderBetween.Item2, cellSize,
                            row.transform, RectTransform.Axis.Vertical);
                    }
                }

            }
        }

        public void LoadLevel(LevelStruct level)
        {
            for(int i = LevelEditor.GRID_SIZE -1; i >= 0; i--)
            {
                for(int j =  LevelEditor.GRID_SIZE - 1; j >= 0; j--)
                    foreach(char tile in level.TileGrid)
                    {
                        
                    }
            }
        }


        private GameObject CreateBorderRow(int rowIndex, float cellSize)
        {
            GameObject borderRow = CreateRowObject(rowIndex, cellSize / BORDER_SCALE);
            GameUtilities.SetParentAndResetPosition(borderRow.transform, rowsLayoutGroup.transform);

            for (int i = 0; i < LevelEditor.GRID_SIZE; i++)
            {
                CreateBorder(new Vector2Int(i, rowIndex), new Vector2Int(i, rowIndex + 1), cellSize, borderRow.transform, RectTransform.Axis.Horizontal);
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

        private LevelEditorBorderObject CreateBorder(Vector2Int positionA, Vector2Int positionB, float cellSize, Transform parent, RectTransform.Axis axis)
        {
            GameObject gameObject = new GameObject($"Border_[{positionA.x},{positionA.y}]-[{positionB.x},{positionB.y}]", typeof(RectTransform));
            LevelEditorBorderObject borderObject = gameObject.AddComponent<LevelEditorBorderObject>();
            borderObject.RectTransform = borderObject.GetComponent<RectTransform>();
            GameUtilities.SetParentAndResetPosition(borderObject.transform, parent);
            AddEmptyImageComponent(borderObject);
            float borderSize = (axis == RectTransform.Axis.Horizontal ? cellSize : cellSize / BORDER_SCALE);
            borderObject.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, borderSize);
            gameObject.layer = LayerMask.NameToLayer("GridCell");
            borderObject.BorderBetweenTiles = new LevelEditorBorderStruct(positionA, positionB);
            BorderDictionary.Add(borderObject.BorderBetweenTiles, borderObject);
            return borderObject;
        }

        private Image AddEmptyImageComponent(LevelEditorCellBase gameObject)
        {
            gameObject.Image = gameObject.AddComponent<Image>();
            gameObject.Image.sprite = Resources.Load<Sprite>("Empty");
            gameObject.Image.color = Color.gray;
            return gameObject.Image;
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
            // Add image
            Image image = cell.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("Empty");
            image.color = Color.gray;

            LevelEditorTileObject cellObject = cell.AddComponent<LevelEditorTileObject>();
            cellObject.GamePosition = new Vector2Int(x, y);
            GameUtilities.SetParentAndResetPosition(cell.transform, parent);

            GameObject entityOnCell = new GameObject("Text", typeof(TextMeshProUGUI));

            GameUtilities.SetParentAndResetPosition(entityOnCell.transform, cell.transform);
            GameUtilities.ResetAnchorsAndSize(entityOnCell.GetComponent<RectTransform>());
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

        private void ProcessCell(Transform cell)
        {
            LevelEditorCellBase cellObject = cell.GetComponent<LevelEditorCellBase>();
            if (cellObject != null)
            {
                bool isValid = true;
                // Initial click
                if (lastPaintedCell == null)
                {
                    lastPaintedCell = cellObject;
                }
                // Follow up
                else if (cellObject != lastPaintedCell && cellObject.GetType() == lastPaintedCell.GetType())
                {
                    lastPaintedCell = cellObject;
                }
                else
                {
                    isValid = false;
                    lastPaintedCell = null;
                }
                if (isValid)
                {
                    CellClickedEvent?.Invoke(cellObject);
                }
            }
        }

    }
}