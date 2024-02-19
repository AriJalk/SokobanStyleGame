using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{
    private static string levelFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\SPG\Levels\";
    public static List<string> GetLevelFiles()
    {
        List<string> fileNames = new List<string>();
        foreach (string file in Directory.GetFiles(levelFolder, "*.lvl"))
        {
            fileNames.Add(Path.GetFileNameWithoutExtension(file));
        }
        return fileNames;
    }

    public static void SaveLevel(string levelJson, string name)
    {
        if (!Directory.Exists(levelFolder))
        {
            Directory.CreateDirectory(levelFolder);
        }
        using (StreamWriter writetext = new StreamWriter(levelFolder + name + ".lvl"))
        {
            writetext.WriteLine(levelJson);
        }
    }

    public static LevelStruct LoadLevel(string levelName)
    {
        string json = string.Empty;
        string path = levelFolder + levelName + ".lvl";
        LevelStruct levelStruct = new LevelStruct();
        if(File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line = string.Empty;
                while(line != null)
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
}