[System.Serializable]
public struct CellPoint
{
    public int x;
    public int y;

    public CellPoint(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"X: {x} Y: {y}";
    }

    public bool Equals(CellPoint p)
    {
        return p.x == this.x && p.y == this.y;
    }

    public static CellPoint CreatChipPoint(int x, int y)
    {
        return new CellPoint(x, y);
    }
}