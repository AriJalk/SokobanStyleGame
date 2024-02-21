using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Handles file loading and saving
/// </summary>
public class FileManager
{
    private static string customLevelsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\SPG\Levels\";
    private static string buildLevelsFolder = Application.streamingAssetsPath + @"\Levels\";


    private static List<string> GetLevelNamesInFolder(string folder)
    {
        List<string> fileNames = new List<string>();
        foreach (string file in Directory.GetFiles(folder, "*.lvl"))
        {
            fileNames.Add(Path.GetFileNameWithoutExtension(file));
        }
        return fileNames;
    }



    private static LevelStruct LoadLevelInPath(string levelPath)
    {
        string json = string.Empty;
        levelPath += ".lvl";
        LevelStruct levelStruct = new LevelStruct();
        if (File.Exists(levelPath))
        {
            using (StreamReader reader = new StreamReader(levelPath))
            {
                string line = string.Empty;
                while (line != null)
                {
                    line = reader.ReadLine();
                    json += line;
                }
            }
            levelStruct = JsonUtility.FromJson<LevelStruct>(json);
            levelStruct.DeserializeFields();
        }
        return levelStruct;
    }

    public static List<string> GetBuildLevelFiles()
    {
        return GetLevelNamesInFolder(buildLevelsFolder);
    }

    public static List<string> GetCustomLevelFiles()
    {
        return GetLevelNamesInFolder(customLevelsFolder);
    }

    public static LevelStruct LoadBuildLevel(string levelName)
    {
        return LoadLevelInPath(buildLevelsFolder + levelName);
    }


    public static LevelStruct LoadCustomLevel(string levelName)
    {
        return LoadLevelInPath(customLevelsFolder + levelName);
    }

    public static void SaveCustomLevel(string levelJson, string name)
    {
        if (!Directory.Exists(customLevelsFolder))
        {
            Directory.CreateDirectory(customLevelsFolder);
        }
        using (StreamWriter writetext = new StreamWriter(customLevelsFolder + name + ".lvl"))
        {
            writetext.WriteLine(levelJson);
        }
    }

    public static void DeleteCustomLevel(string name)
    {
        string path = customLevelsFolder + name + ".lvl";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}