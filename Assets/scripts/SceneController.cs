using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SceneController : MonoBehaviour
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
    private Chip originalEmptyCircle;

    [SerializeField]
    private int rows;

    [SerializeField]
    private int cols;

    [SerializeField]
    private Vector2 gridSize;

    [SerializeField]
    private Vector2 gridOffset;

    private Chip chosenChip;
    private Vector2 cellSize;
    private Vector2 cellScale;

    public Sprite cellSprite;
    private Chip.ChipPoint enterPoint;
    private Chip.ChipPoint exitPoint;
    private Chip[,] chips;

    void Start()
    {
        enterPoint.x = rows - 1;
        enterPoint.y = 0;

        chips = new Chip[rows, cols];
        BuildChipGrid();
    }



    public void ChooseChip(Chip chosen)
    {
        if (chosenChip != null)
        {
            chosenChip.ActivateChosenChip(false);
        }
        chosenChip = chosen;
    }

    public void ClickOnControl(ChipType type)
    {
        chosenChip.chip_type = type;
        chosenChip.SetChip(SetChipSpriteByType(type));
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                CheckChip(x, y);
            }
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
                chip.controller = this;
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
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }



    private void CheckBoard(int x, int y)
    {
        Chip currentChip = chips[x, y];
        Debug.Log(currentChip.isEnterPoint + " " + x + " " + y);
        List<ChipType> leftTypes = new List<ChipType>();
        List<ChipType> rightTypes = new List<ChipType>();
        switch (currentChip.chip_type)
        {
            case ChipType.HORIZONTAL:

                Chip leftChip = null;
                Chip rightChip = null;

                if (y != 0)
                {
                    leftChip = chips[x, y - 1];
                }

                if (y != cols - 1)
                {
                    rightChip = chips[x, y + 1];
                }

                leftTypes.Add(ChipType.HORIZONTAL);
                leftTypes.Add(ChipType.RIGHT_DOWN);
                leftTypes.Add(ChipType.UP_RIGHT);

                rightTypes.Add(ChipType.HORIZONTAL);
                rightTypes.Add(ChipType.LEFT_DOWN);
                rightTypes.Add(ChipType.LEFT_UP);

                if ((leftChip != null && leftChip.isCheck && leftTypes.Contains(leftChip.chip_type)) || (rightChip != null && rightChip.isCheck && rightTypes.Contains(rightChip.chip_type)))
                {
                    currentChip.SetChipCheck(true);
                }
                else
                {
                    currentChip.SetChipCheck(false);
                }
                break;
            default:
                currentChip.SetChipCheck(false);
                break;
        }
    }

    private void CheckChip(int x, int y)
    {
        Chip currentChip = chips[x, y];

        Chip left = null;
        Chip right = null;

        List<ChipType> left_side = new List<ChipType>();
        List<ChipType> right_side = new List<ChipType>();

        bool is_check = false;

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

                is_check = (left != null && left.isCheck && left_side.Contains(left.chip_type))
                    || (right != null && right.isCheck && right_side.Contains(right.chip_type));
                currentChip.SetChipCheck(is_check);
                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                break;

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

                is_check = (left != null && left.isCheck && left_side.Contains(left.chip_type))
                    || (right != null && right.isCheck && right_side.Contains(right.chip_type));

                currentChip.SetChipCheck(is_check);
                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                break;

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

                is_check = (left != null && left.isCheck && left_side.Contains(left.chip_type))
                    || (right != null && right.isCheck && right_side.Contains(right.chip_type));

                currentChip.SetChipCheck(is_check);
                left_side.Clear();
                right_side.Clear();
                left_side = null;
                right_side = null;
                break;
        }
    }


}
