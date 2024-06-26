using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SPG.LevelEditor
{
    /// <summary>
    /// Handles level creation by converting the elements in the ui to a LevelStruct json format
    /// </summary>
    public class LevelEditor : MonoBehaviour
    {
        public const short GRID_SIZE = 7;


        [SerializeField]
        private LevelEditorUI ui;
        [SerializeField]
        private ToggleGroupManager toggleGroupManager;

        private char[,] tileGrid = new char[GRID_SIZE, GRID_SIZE];
        private char[,] entityGrid = new char[GRID_SIZE, GRID_SIZE];
        private List<LevelEditorBorderStruct> borderList;

        private UnityEvent<LevelEditorCellBase> cellClickedEvent;
        private UnityEvent<ButtonEvents> buttonEvent;
        // Start is called before the first frame update
        void Start()
        {
            cellClickedEvent = ui.CellClickedEvent;
            cellClickedEvent.AddListener(ProcessCellInput);
            buttonEvent = ui.ButtonEvent;
            buttonEvent.AddListener(ProccessButtonCommand);
            ui.BuildGrid();
            borderList = new List<LevelEditorBorderStruct>();
            if (StaticManager.GameState == GameState.Edit && !StaticManager.LevelName.Equals(string.Empty))
            {
                LevelStruct level = FileManager.LoadCustomLevel(StaticManager.LevelName);
                ui.InputField.text = StaticManager.LevelName;
                level.DeserializeFields();
                tileGrid = level.TileGrid;
                entityGrid = level.EntityGrid;
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    for (int j = 0; j < GRID_SIZE; j++)
                    {
                        SetTile(ui.TileObjectGrid[j, i], tileGrid[j, i]);
                        SetEntity(ui.TileObjectGrid[j, i], entityGrid[j, i]);
                    }
                }
                for (int i = 0; i < level.BorderList.Count; i++)
                {
                    LevelEditorBorderStruct border = level.BorderList[i];
                    ProccessAddBorder(ui.BorderDictionary[border]);
                }

            }


        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            cellClickedEvent?.RemoveListener(ProcessCellInput);
            buttonEvent?.RemoveListener(ProccessButtonCommand);
        }

        /// <summary>
        /// Main input management method
        /// </summary>
        /// <param name="cell"></param>
        private void ProcessCellInput(LevelEditorCellBase cell)
        {
            IEnumerable<Toggle> paintModeToggle = toggleGroupManager.PaintModeToggleOptions.ActiveToggles();

            foreach (Toggle toggle in paintModeToggle)
            {
                switch (toggle.name)
                {
                    case "AddOption":
                        ProcessAdd(toggleGroupManager.ActorSelectionOptions.ActiveToggles(), cell);
                        break;
                    case "RemoveOption":
                        ProccessRemove(cell);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Main Add method
        /// </summary>
        /// <param name="actorToggles"></param>
        /// <param name="cell"></param>
        private void ProcessAdd(IEnumerable<Toggle> actorToggles, LevelEditorCellBase cell)
        {
            LevelEditorTileObject tile = cell.GetComponent<LevelEditorTileObject>();

            foreach (Toggle toggle in actorToggles)
            {
                switch (toggle.name)
                {
                    case "TileOption":
                        if (tile != null)
                            ProcessAddTile(toggleGroupManager.TileVariants.ActiveToggles(), tile);
                        break;
                    case "EntityOption":
                        if (tile != null)
                            ProcessAddEntity(toggleGroupManager.EntityVariants.ActiveToggles(), tile);
                        break;
                    case "BorderOption":
                        LevelEditorBorderObject border = cell.GetComponent<LevelEditorBorderObject>();
                        if (border != null)
                        {
                            ProccessAddBorder(border);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Main Remove Method
        /// </summary>
        /// <param name="cell"></param>
        private void ProccessRemove(LevelEditorCellBase cell)
        {
            LevelEditorTileObject tile = cell.GetComponent<LevelEditorTileObject>();
            foreach (Toggle toggle in toggleGroupManager.ActorSelectionOptions.ActiveToggles())
            {
                switch (toggle.name)
                {
                    case "TileOption":
                        if (tile != null)
                        {
                            tile.GetComponent<Image>().color = Color.gray;
                            tileGrid[tile.GamePosition.x, tile.GamePosition.y] = '\0';
                        }

                        break;
                    case "EntityOption":
                        if (tile != null)
                        {
                            tile.EntityOnTile.text = '\0'.ToString();
                            entityGrid[tile.GamePosition.x, tile.GamePosition.y] = '\0';
                        }
                        break;
                    case "BorderOption":
                        LevelEditorBorderObject border = cell.GetComponent<LevelEditorBorderObject>();
                        if (border != null)
                        {
                            ProccessRemoveBorder(border);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void ProcessAddTile(IEnumerable<Toggle> typeToggles, LevelEditorTileObject cell)
        {
            foreach (Toggle toggle in typeToggles)
            {
                switch (toggle.name)
                {
                    case "BasicTile":
                        SetTile(cell, 't');
                        break;
                    case "RedGoalTile":
                        SetTile(cell, 'r');
                        break;
                    case "BlueGoalTile":
                        SetTile(cell, 'b');
                        break;
                    default:
                        break;
                }
            }
        }



        private void SetTile(LevelEditorTileObject cell, char tileType)
        {
            tileGrid[cell.GamePosition.x, cell.GamePosition.y] = tileType;
            Image image = cell.GetComponent<Image>();
            if (image != null)
            {
                switch (tileType)
                {
                    case 't':
                        image.color = Color.black;
                        break;
                    case 'r':
                        image.color = Color.red;
                        break;
                    case 'b':
                        image.color = Color.blue;
                        break;
                    default:
                        break;
                }
            }
            PrintCells();
        }



        private void ProcessAddEntity(IEnumerable<Toggle> typeToggles, LevelEditorTileObject cell)
        {
            foreach (Toggle toggle in typeToggles)
            {
                switch (toggle.name)
                {
                    case "Player":
                        SetEntity(cell, 'p');
                        break;
                    case "RedCube":
                        SetEntity(cell, 'r');
                        break;
                    case "BlueCube":
                        SetEntity(cell, 'b');
                        break;
                    case "Sphere":
                        SetEntity(cell, 's');
                        break;
                    default:
                        break;
                }
            }
        }

        public void ProccessAddBorder(LevelEditorBorderObject border)
        {
            if (!borderList.Contains(border.BorderBetweenTiles))
            {
                borderList.Add(border.BorderBetweenTiles);
                border.Image.color = Color.magenta;
            }
        }

        public void ProccessRemoveBorder(LevelEditorBorderObject border)
        {
            if (borderList.Contains(border.BorderBetweenTiles))
            {
                borderList.Remove(border.BorderBetweenTiles);
                border.Image.color = Color.gray;
            }
        }

        private void SetEntity(LevelEditorTileObject cell, char entityChar)
        {
            if (entityChar == 'x')
                entityChar = '\0';
            cell.EntityOnTile.text = entityChar.ToString();
            //Debug.Log(entityChar.ToString());
            
            entityGrid[cell.GamePosition.x, cell.GamePosition.y] = entityChar;
            PrintCells();
        }

        private void PrintCells()
        {
            string map = string.Empty;
            string entities = string.Empty;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (tileGrid[i, j] == '\0')
                        map += "x";
                    else
                    {
                        map += tileGrid[i, j];
                    }
                    if (entityGrid[i, j] == '\0')
                        entities += "x";
                    else
                    {
                        entities += entityGrid[i, j];
                    }
                }
                map += '\n';
                entities += "\n";
            }
            //Debug.Log("map\n" + map);
            //Debug.Log("entities\n" + entities);
        }

        private void SaveLevel()
        {
            //borderList.Add(new LevelEditorBorderStruct(Vector2Int.zero, Vector2Int.up));
            //borderList.Add(new LevelEditorBorderStruct(new Vector2Int(3,3), new Vector2Int(4,3)));

            LevelStruct levelStruct = new LevelStruct(tileGrid, entityGrid, borderList);
            levelStruct.SerializeFields();
            string json = JsonUtility.ToJson(levelStruct);
            //Test_Json();
            //GameUtilities.CopyStringToClipboard(json);
            if (ui.InputField.text != string.Empty)
                FileManager.SaveCustomLevel(json, ui.InputField.text);
        }

        private void ExitEditor()
        {
            SceneManager.LoadScene(0);
        }

        private void ProccessButtonCommand(ButtonEvents clickedEvent)
        {
            switch (clickedEvent)
            {
                case ButtonEvents.Save:
                    SaveLevel();
                    break;
                case ButtonEvents.Exit:
                    ExitEditor();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check if saved level matches loaded level
        /// </summary>
        /// <returns></returns>
        private bool TestJson()
        {
            LevelStruct levelStruct = new LevelStruct(tileGrid, entityGrid, borderList);
            levelStruct.SerializeFields();
            string originalJson = JsonUtility.ToJson(levelStruct);
            Debug.Log("Original JSON\n" + originalJson);

            levelStruct = JsonUtility.FromJson<LevelStruct>(originalJson);
            levelStruct.DeserializeFields();
            levelStruct.SerializeFields();

            string secondJson = JsonUtility.ToJson(levelStruct);
            Debug.Log("Second JSON\n" + secondJson);

            if (string.Compare(originalJson, secondJson) == 0)
            {
                Debug.Log("Test Passed OK");
                return true;
            }
            Debug.Log("Level JSON not equal");
            return false;
        }

    }

}
