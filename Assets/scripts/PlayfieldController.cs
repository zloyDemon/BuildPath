using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldController : MonoBehaviour
{
    [SerializeField]
    private Chip originalEmptyCircle;

    [SerializeField]
    private Vector2 gridSize;

    [SerializeField]
    private Vector2 gridOffset;

    [SerializeField]
    private int rows;

    [SerializeField]
    private int cols;

    public Sprite cellSprite;
    private Chip[,] chips;

    private Vector2 cellSize;
    private Vector2 cellScale;
    public ChipPoint enterPoint;
    public ChipPoint exitPoint;

    private void InitField()
    {
        chips = new Chip[rows, cols];
        cellSize = cellSprite.bounds.size;

        Debug.Log(cellSize);

        Vector3 newCellSize = new Vector3(gridSize.x / (float) cols, gridSize.y / (float) rows);

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
                Vector3 pos = new Vector3(col * cellSize.x + gridOffset.x + transform.position.x,
                    row * cellSize.y + gridOffset.y + transform.position.y);
                Chip chip = Instantiate(originalEmptyCircle) as Chip;
                chip.Init(new ChipPoint((rows - 1) - row, (cols - 1) - col));
                //chip.manager = this;
                chip.transform.position = pos;
                chip.transform.parent = transform;

                if (row == enterPoint.x && col == enterPoint.y)
                {

                }

                chips[(rows - 1) - row, (cols - 1) - col] = chip;
            }
        }

        /*foreach (Chip.ChipPoint point in block_point)
        {
            //chips[point.x, point.y].SetChip(SetChipSpriteByType(ChipType.Block), ChipType.Block);
        }*/
    }
}