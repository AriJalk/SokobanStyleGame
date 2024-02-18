using System.Collections.Generic;
using System.IO;

public class FileManager
{
    private static string levelFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\SPG\Levels";
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
        using (StreamWriter writetext = new StreamWriter(levelFolder + @"\" + name + ".lvl"))
        {
            writetext.WriteLine(levelJson);
        }
    }
}