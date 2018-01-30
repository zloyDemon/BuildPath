using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BPManager : MonoBehaviour
{
    enum Side
    {
        FIRST_SIDE,
        SECOND_SIDE
    }

    private const int MAX_ROW_VALUE = 4;
    private const int MAX_COLUMN_VALUE = MAX_ROW_VALUE;



    [SerializeField]
    private Chip emptySprite;

    [SerializeField]
    private Sprite horizontal;

    [SerializeField]
    private Sprite vertical;

    [SerializeField]
    private Sprite up_right;

    [SerializeField]
    private Sprite right_down;

    [SerializeField]
    private Sprite left_down;

    [SerializeField]
    private Sprite left_up;

    [SerializeField]
    private Sprite empty;

    [SerializeField]
    private Sprite block;

    [SerializeField]
    private Chip originalEmptyCircle;

    [SerializeField]
    private int rows;

    [SerializeField]
    private int cols;

    [SerializeField]
    private Vector2 gridSize;

    [SerializeField]
    private Vector2 gridOffset;

    public Chip.ChipPoint[] block_point;

    private Chip chosenChip;
    private Chip checkCHip;

    private bool isGameEnd;

    private Vector2 cellSize;
    private Vector2 cellScale;

    public Sprite cellSprite;

    public Chip.ChipPoint enterPoint;
    public Chip.ChipPoint exitPoint;

    private Chip[,] chips;

    private GameTimer gameTimer;


    void Start()
    {
        isGameEnd = false;
        gameTimer = GetComponent<GameTimer>();
        gameTimer.StartTime();
        chips = new Chip[rows, cols];
        BuildChipGrid();
    }


    public void ChooseChip(Chip chosen)
    {
        if (!isGameEnd)
        {
            if (chosenChip != null)
            {
                chosenChip.ActivateChosenChip(false);
            }
            chosenChip = chosen;
            chosenChip.ActivateChosenChip(true);
        }
    }

    public void ClickOnControl(ChipType type)
    {
        if (!isGameEnd)
        {
            chosenChip.SetChip(SetChipSpriteByType(type),type);

            foreach (Chip chip in chips)
            {
                chip.SetChipCheck(false);
            }
            CheckGame();
           
            Debug.Log(checkCHip.chipPoint.x + " " + checkCHip.chipPoint.y);
        }

    }

    private void CheckGame()
    {
        checkCHip = chips[enterPoint.x, enterPoint.y];
        Chip res = CheckGameBoard(checkCHip);
        while (res != null)
        {
            checkCHip = res;
            checkCHip.SetChipCheck(true);
            res = CheckGameBoard(checkCHip);
        }

        if(checkCHip.chipPoint.x == exitPoint.x && checkCHip.chipPoint.y == exitPoint.y)
        {
            Debug.Log("Win!");
            isGameEnd = true;
            chosenChip.ActivateChosenChip(false);
        }
    }



    private Sprite SetChipSpriteByType(ChipType type)
    {
        Sprite sprite = null;
        switch (type)
        {
            case ChipType.HORIZONTAL:
                sprite = horizontal;
                break;
            case ChipType.VERTICAL:
                sprite = vertical;
                break;
            case ChipType.LEFT_DOWN:
                sprite = left_down;
                break;
            case ChipType.RIGHT_DOWN:
                sprite = right_down;
                break;
            case ChipType.UP_RIGHT:
                sprite = up_right;
                break;
            case ChipType.LEFT_UP:
                sprite = left_up;
                break;
            case ChipType.BLOCK:
                sprite = block;
                break;
            case ChipType.EMPTY:
            default:
                sprite = empty;
                break;
        }
        return sprite;
    }



    private void BuildChipGrid()
    {
        cellSize = cellSprite.bounds.size;

        Debug.Log(cellSize);

        Vector3 newCellSize = new Vector3(gridSize.x / (float)cols, gridSize.y / (float)rows);

        cellScale.x = newCellSize.x / cellSize.x;
        cellScale.y = newCellSize.y / cellSize.y;

        cellSize = newCellSize;

        originalEmptyCircle.transform.localScale = new Vector3(cellScale.x, cellScale.y, 0);

        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;


        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = new Vector3(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);
                Chip chip = Instantiate(originalEmptyCircle) as Chip;
                chip.SetChipPoint(new Chip.ChipPoint(row, col));
                chip.manager = this;
                chip.transform.position = pos;
                chip.transform.parent = transform;
                chip.isEnterPoint = false;

                if (row == enterPoint.x && col == enterPoint.y)
                {
                    chip.isEnterPoint = true;
                    chosenChip = chip;
                }

                chips[row, col] = chip;
            }
        }

        foreach (Chip.ChipPoint point in block_point)
        {
            chips[point.x, point.y].SetChip(SetChipSpriteByType(ChipType.BLOCK), ChipType.BLOCK);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }

    private Chip CheckGameBoard(Chip currentChip)
    {
        int x = currentChip.chipPoint.x;
        int y = currentChip.chipPoint.y;

        Chip left = null;
        Chip right = null;

        Chip result = null;

        List<ChipType> left_side = new List<ChipType>();
        List<ChipType> right_side = new List<ChipType>();


        switch (currentChip.chip_type)
        {
            case ChipType.HORIZONTAL:

                if (y != 0)
                {
                    left = chips[x, y - 1];
                }

                if (y != cols - 1)
                {
                    right = chips[x, y + 1];
                }

                left_side.Add(ChipType.HORIZONTAL);
                left_side.Add(ChipType.RIGHT_DOWN);
                left_side.Add(ChipType.UP_RIGHT);

                right_side.Add(ChipType.HORIZONTAL);
                right_side.Add(ChipType.LEFT_DOWN);
                right_side.Add(ChipType.LEFT_UP);

                if (right != null && !right.isCheck && right_side.Contains(right.chip_type))
                {
                    result = right;
                }

                else if (left != null && !left.isCheck && left_side.Contains(left.chip_type))
                {
                    result = left;
                }

                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                return result;

            case ChipType.VERTICAL:

                if (x != 0)
                {
                    left = chips[x - 1, y];
                }

                if (x != rows - 1)
                {
                    right = chips[x + 1, y];
                }

                left_side.Add(ChipType.UP_RIGHT);
                left_side.Add(ChipType.LEFT_UP);
                left_side.Add(ChipType.VERTICAL);

                right_side.Add(ChipType.VERTICAL);
                right_side.Add(ChipType.RIGHT_DOWN);
                right_side.Add(ChipType.LEFT_DOWN);

                if (right != null && !right.isCheck && right_side.Contains(right.chip_type))
                {
                    result = right;
                }

                else if (left != null && !left.isCheck && left_side.Contains(left.chip_type))
                {
                    result = left;
                }

                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                return result;

            case ChipType.LEFT_DOWN:

                if (y != 0)
                {
                    left = chips[x, y - 1];
                }

                if (x != 0)
                {
                    right = chips[x - 1, y];
                }

                left_side.Add(ChipType.HORIZONTAL);
                left_side.Add(ChipType.RIGHT_DOWN);
                left_side.Add(ChipType.UP_RIGHT);

                right_side.Add(ChipType.VERTICAL);
                right_side.Add(ChipType.UP_RIGHT);
                right_side.Add(ChipType.LEFT_UP);

                if (right != null && !right.isCheck && right_side.Contains(right.chip_type))
                {
                    result = right;
                }

                else if (left != null && !left.isCheck && left_side.Contains(left.chip_type))
                {
                    result = left;
                }

                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                return result;


            case ChipType.LEFT_UP:

                if (y != 0)
                {
                    left = chips[x, y - 1];
                }

                if (x != rows - 1)
                {
                    right = chips[x + 1, y];
                }

                left_side.Add(ChipType.UP_RIGHT);
                left_side.Add(ChipType.HORIZONTAL);
                left_side.Add(ChipType.RIGHT_DOWN);

                right_side.Add(ChipType.VERTICAL);
                right_side.Add(ChipType.RIGHT_DOWN);
                right_side.Add(ChipType.LEFT_DOWN);

                if (right != null && !right.isCheck && right_side.Contains(right.chip_type))
                {
                    result = right;
                }

                else if (left != null && !left.isCheck && left_side.Contains(left.chip_type))
                {
                    result = left;
                }

                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                return result;

            case ChipType.UP_RIGHT:

                if (x != rows - 1)
                {
                    left = chips[x + 1, y];
                }

                if (y != cols - 1)
                {
                    right = chips[x, y + 1];
                }

                left_side.Add(ChipType.VERTICAL);
                left_side.Add(ChipType.RIGHT_DOWN);
                left_side.Add(ChipType.LEFT_DOWN);

                right_side.Add(ChipType.HORIZONTAL);
                right_side.Add(ChipType.LEFT_DOWN);
                right_side.Add(ChipType.LEFT_UP);

                if (right != null && !right.isCheck && right_side.Contains(right.chip_type))
                {
                    result = right;
                }

                else if (left != null && !left.isCheck && left_side.Contains(left.chip_type))
                {
                    result = left;
                }

                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                return result;

            case ChipType.RIGHT_DOWN:

                if (x != 0)
                {
                    left = chips[x - 1, y];
                }

                if (y != cols - 1)
                {
                    right = chips[x, y + 1];
                }

                left_side.Add(ChipType.VERTICAL);
                left_side.Add(ChipType.LEFT_UP);
                left_side.Add(ChipType.UP_RIGHT);

                right_side.Add(ChipType.HORIZONTAL);
                right_side.Add(ChipType.LEFT_DOWN);
                right_side.Add(ChipType.LEFT_UP);

                if (right != null && !right.isCheck && right_side.Contains(right.chip_type))
                {
                    result = right;
                }

                else if (left != null && !left.isCheck && left_side.Contains(left.chip_type))
                {
                    result = left;
                }

                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                return result;
        }
        return null;
    }

}
