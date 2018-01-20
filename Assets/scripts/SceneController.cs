using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

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

    void Start ()
    {
        BuildChipGrid();
    }

    private void Update()
    {
        
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
                chip.SetChipPoint(new Chip.ChipPoint(row,col));
                chip.controller = this;
                chip.transform.position = pos;
                chip.transform.parent = transform;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,gridSize);
    }

    public void ChooseChip(Chip chosen)
    {
        if(chosenChip != null)
        {
            chosenChip.DeactivateChosenChip();
        }      
        chosenChip = chosen;
    }

    public void ClickOnControl(ChipType type)
    {
        if(chosenChip != null)
        {
            chosenChip.SetChip(SetChipSpriteByType(type), type);
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
        }
        return sprite;
    }
}
