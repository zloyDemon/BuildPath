using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;

public class LevelData
{
    public int FieldId { get;  set; }
    public CellType[,] Field { get;  set; }
    public Dictionary<CellType, int> RightCells { get;  set; }
    public int EnterPointIndex { get;  set; }
    public int ExitPointIndex { get;  set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
