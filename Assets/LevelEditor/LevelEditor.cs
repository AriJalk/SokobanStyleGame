using SPG.LevelEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    private enum Toggles
    {
        Tile,
        BasicTile,
        RedGoal,
        BlueGoal,
        Entity,
        Player,
        RedCube,
        BlueCube,
        Sphere,
    }

    const short GRID_SIZE = 7;
    [SerializeField]
    private LevelEditorUI ui;
    [SerializeField]
    private ToggleGroupManager toggleGroupManager;

    private char[,] mapGrid = new char[GRID_SIZE, GRID_SIZE];
    private char[,] entityGrid = new char[GRID_SIZE, GRID_SIZE];

    private UnityEvent<GridCellObject> cellClickedEvent;
    private UnityEvent saveEvent;
    // Start is called before the first frame update
    void Start()
    {
        cellClickedEvent = ui.CellClickedEvent;
        cellClickedEvent.AddListener(ProcessCellInput);
        saveEvent = ui.SaveEvent;
        saveEvent.AddListener(SaveLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        cellClickedEvent?.RemoveListener(ProcessCellInput);
    }

    private void ProcessCellInput(GridCellObject cell)
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

    private void ProcessAdd(IEnumerable<Toggle> actorToggles, GridCellObject cell)
    {
        foreach (Toggle toggle in actorToggles)
        {
            switch (toggle.name)
            {
                case "TileOption":
                    ProcessAddTile(toggleGroupManager.TileVariants.ActiveToggles(), cell);
                    break;
                case "EntityOption":
                    ProcessAddEntity(toggleGroupManager.EntityVariants.ActiveToggles(), cell);
                    break;
                default:
                    break;
            }
        }
    }

    private void ProcessAddTile(IEnumerable<Toggle> typeToggles, GridCellObject cell)
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

    private void SetTile(GridCellObject cell, char tileType)
    {
        mapGrid[cell.GamePosition.x, cell.GamePosition.y] = tileType;
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

    private void ProccessRemove(GridCellObject cell)
    {
        foreach (Toggle toggle in toggleGroupManager.ActorSelectionOptions.ActiveToggles())
        {
            switch (toggle.name)
            {
                case "TileOption":
                    cell.GetComponent<Image>().color = Color.gray;
                    mapGrid[cell.GamePosition.x, cell.GamePosition.y] = '\0';
                    break;
                case "EntityOption":
                    cell.EntityOnTile.text = '\0'.ToString();
                    entityGrid[cell.GamePosition.x, cell.GamePosition.y] = '\0';
                    break;
                default:
                    break;
            }
        }
    }

    private void ProcessAddEntity(IEnumerable<Toggle> typeToggles, GridCellObject cell)
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

    private void SetEntity(GridCellObject cell, char entityChar)
    {
        cell.EntityOnTile.text = entityChar.ToString();
        Debug.Log(entityChar.ToString());
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
                if (mapGrid[i, j] == '\0')
                    map += "x";
                else
                {
                    map += mapGrid[i, j];
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
        Debug.Log("map\n" + map);
        Debug.Log("entieis\n" + entities);
    }

    private void SaveLevel()
    {
        LevelStruct levelStruct = new LevelStruct(mapGrid, entityGrid, new List<System.Tuple<Vector2Int, Vector2Int>>());
        levelStruct.SerializeFields();
        string json = JsonUtility.ToJson(levelStruct);
        Debug.Log(json);
        Test_Json();
    }

    private void LoadLevel()
    {

    }

    private bool Test_Json()
    {
        LevelStruct levelStruct = new LevelStruct(mapGrid, entityGrid, new List<System.Tuple<Vector2Int, Vector2Int>>());
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
