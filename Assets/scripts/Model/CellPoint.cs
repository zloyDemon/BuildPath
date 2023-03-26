[System.Serializable]
public struct CellPoint
{
    public int row;
    public int column;

    public CellPoint(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public override string ToString()
    {
        return $"X: {row} Y: {column}";
    }

    public bool Equals(CellPoint p)
    {
        return p.row == this.row && p.column == this.column;
    }

    public static CellPoint CreatChipPoint(int row, int column)
    {
        return new CellPoint(row, column);
    }
}