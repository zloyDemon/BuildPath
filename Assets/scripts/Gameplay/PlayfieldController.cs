using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;
using Zenject;

public class PlayfieldController : MonoBehaviour
{
    private const float EdgePointsOffset = 1.6f;
    
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private Vector2 gridOffset;
    [SerializeField] public Sprite cellSprite;
    [SerializeField] private Chip originalEmptyChip;
    [SerializeField] private SpriteRenderer edgePoint;
    [SerializeField] private SpriteHolder _chipsSpriteHolder;

    private Chip _enterPoint;
    private Chip _exitPoint;
    private Vector2 cellSize;
    private Vector2 cellScale;
    private Chip[,] chips;
    private Chip _currentChosenChip;
    private CellsController cellsController;

    public Chip[,] Chips => chips;
    public Chip EnterPoint => _enterPoint;
    public Chip ExitPoint => _exitPoint;
    public IEnumerable<Chip> RightChips => cellsController.RightChips;
    
    public event Action OnCheckCompleted;

    public void Init(GameProcessManager manager)
    {
        cellsController = new CellsController();
        cellsController.OnCheckCompleted += CellsControllerOnOnCheckCompleted;
        BuildChipGrid();
    }

    private void CellsControllerOnOnCheckCompleted(CellsController.CheckStatus status)
    {
        if (status == CellsController.CheckStatus.Success)
            Debug.Log("Win");
    }

    public Sprite GetChipSpriteByType(CellType type)
    {
        return GetChipSpriteByName(type.ToString());
    }
    
    public Sprite GetChipSpriteByName(string name)
    {
        return _chipsSpriteHolder.GetSpriteByName(name);
    }
    
    public void ChoseTypeByControl(CellType type)
    {
        if (_currentChosenChip != null)
        {
            _currentChosenChip.SetChipData(type, GetChipSpriteByType(type));
            
            foreach (var chip in Chips)
                chip.SetChipOnWay(false); // Todo
            
            cellsController.CheckGame();
        }
    }
    
    private void BuildChipGrid()
    {
        cellSize = cellSprite.bounds.size;

        var field = FieldGenerator.GetField();

        int rows = field.GetLength(0);
        int cols = field.GetLength(1);
        
        Vector3 newCellSize = new Vector3(gridSize.x / (float)cols, gridSize.y / (float)rows);
        chips = new Chip[rows, cols];
        cellScale.x = newCellSize.x / cellSize.x;
        cellScale.y = newCellSize.y / cellSize.y;

        cellSize = newCellSize;

        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;


        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = new Vector3(col * cellSize.x + gridOffset.x + transform.position.x, -(row * cellSize.y + gridOffset.y - transform.position.y));
                Chip chip = Instantiate(originalEmptyChip, transform, true);
                chip.gameObject.name = $"{chip.gameObject.name}_{col}_{row}"; 
                chip.Init(new CellPoint(col, row));
                chip.SetClickListener(OnChipClick);
                chip.transform.position = pos;
                chip.SetChipColor(Color.gray);
                var id = field[row, col];
                var type = GetTypeById(id);
                chip.SetChipData(type, GetChipSpriteByType(type));
                chips[col, row] = chip;
                
                // Todo
                if (id == 1)
                {
                    _enterPoint = chip;
                }
                else if (id == 3)
                {
                    _exitPoint = chip;
                }
            }
        }

        var enterPoint = Instantiate(edgePoint);
        var exitPoint = Instantiate(edgePoint);
        
        enterPoint.transform.position = _enterPoint.transform.position + (Vector3.left * EdgePointsOffset);
        exitPoint.transform.position = _exitPoint.transform.position + (Vector3.right * EdgePointsOffset);
        var newScale = exitPoint.transform.localScale;
        newScale.x *= -1;
        exitPoint.transform.localScale = newScale;
        
        cellsController.SetCells(chips, _enterPoint, _exitPoint);
    }

    private void OnChipClick(Chip chip)
    {
        if (chip.CellType == CellType.Block)
            return;
        
        if (_currentChosenChip != null)
            _currentChosenChip.SetSelect(false);

        _currentChosenChip = chip;
        _currentChosenChip.SetSelect(true);
    }

    private CellType GetTypeById(int id)
    {
        switch (id)
        {
            case 0:
            case 1:
            case 3: return CellType.Empty;
            case 2: return CellType.Block;
            default: throw new Exception($"ChipType by id = {id} not found");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }
}