using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;

interface ICell
{
    CellType CellType { get; }
    bool IsOnWay { get; }
}
