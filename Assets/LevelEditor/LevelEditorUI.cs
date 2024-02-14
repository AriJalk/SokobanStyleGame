﻿using System;
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
        public UnityEvent<GridCellObject> CellClickedEvent;
        public UnityEvent<ButtonEvents> ButtonEvent;
        [SerializeField]
        private FlexibleGrid LevelGrid;
        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button exitButton;

        private Vector2Int lastPosition;
        private Button clickedButton;

        private void Awake()
        {
            BuildGrid();
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

        private void OnDestroy()
        {
            saveButton.onClick.RemoveListener(SaveClicked);
            exitButton.onClick.RemoveListener(ExitClicked);
        }

        private void BuildGrid()
        {
            for(int i = 0; i < LevelGrid.GridConstraint; i++)
            {
                for (int j = 0; j < LevelGrid.GridConstraint; j++)
                {
                    GameObject cell = new GameObject($"Cell_[{j},{i}]", typeof(RectTransform));
                    cell.layer = LayerMask.NameToLayer("GridCell");
                    Image image = cell.AddComponent<Image>();
                    image.sprite = Resources.Load<Sprite>("Empty");
                    image.color = Color.gray;

                    GridCellObject cellObject = cell.AddComponent<GridCellObject>();
                    // Transform so that the result matrix is rotated 90 degress in data to corespond to X,Y
                    cellObject.GamePosition = new Vector2Int(j, LevelGrid.GridConstraint - i -1);
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

        private void SaveClicked()
        {
            ButtonEvent?.Invoke(ButtonEvents.Save);
        }

        private void ExitClicked()
        {
            ButtonEvent?.Invoke(ButtonEvents.Exit);
        }
    }
}