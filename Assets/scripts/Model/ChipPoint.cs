[System.Serializable]
public struct ChipPoint
{
    public int x;
    public int y;

    public ChipPoint(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"X: {x} Y: {y}";
    }

    public static ChipPoint CreatChipPoint(int x, int y)
    {
        return new ChipPoint(x, y);
    }
}