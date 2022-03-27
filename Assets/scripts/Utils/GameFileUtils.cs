using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameFileUtils
{
    private const string FilePath = "Assets/Resources/levels_data.json";

    public static void WriteToFile(string text)
    {
        File.WriteAllText(FilePath, text);
    }

    public static string ReadFromFile()
    {
        return File.ReadAllText(FilePath);
    }
}
