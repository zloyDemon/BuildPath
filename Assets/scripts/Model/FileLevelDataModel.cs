using System.Collections.Generic;

public class FileLevelDataModel
{
    public int fieldId;
    public int[] field;
    public Dictionary<int, int> rightCells;
    public int enterPointIndex;
    public int exitPointIndex;

    public FileLevelDataModel(int fieldId, int enterPointIndex, int exitPointIndex, int[] field, Dictionary<int, int> rightCells)
    {
        this.fieldId = fieldId;
        this.enterPointIndex = enterPointIndex;
        this.exitPointIndex = exitPointIndex;
        this.field = field;
        this.rightCells = rightCells;
    }
}
