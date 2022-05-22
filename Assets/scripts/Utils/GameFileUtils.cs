using System.Collections;
using System.Collections.Generic;
using System.IO;
using Enum;
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

    public void SaveFieldToFile(List<LevelEditorCell> cells, int enterPointIndex, int exitPointIndex)
    {
        var rightCells = new Dictionary<int, int>();
        int[] fieldCells = new int[cells.Count];
        
        for(int i = 0; i < cells.Count; i++)
        {
            var cell = cells[i];

            fieldCells[i] = (int)cell.CellType;
            
            var cellType = cell.CellType;
            if (cellType == CellType.Block || cellType == CellType.Empty)
            {
                continue;
            }

            int cellId = (int)cellType;

            if (!rightCells.ContainsKey(cellId))
            {
                rightCells.Add(cellId, 0);    
            }

            rightCells[cellId]++;
        }
    }
}
