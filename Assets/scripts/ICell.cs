using Enum;

interface ICell
{
    CellType CellType { get; }
    bool IsOnWay { get; }
}
