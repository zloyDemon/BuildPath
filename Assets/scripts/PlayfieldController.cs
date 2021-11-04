using System;
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

    private BPManager _bpManager;
    
    private Chip _enterPoint;
    private Chip _exitPoint;
    private Vector2 cellSize;
    private Vector2 cellScale;
    private Chip[,] chips;

    public Chip[,] Chips => chips;
    public Chip EnterPoint => _enterPoint;
    public Chip ExitPoint => _exitPoint;
    
    public event Action<Chip> OnChipClicked;

    public void Init()
    {
        _bpManager = BPManager.Instance;
        BuildChipGrid();
    }
    
    public bool EnterOrExitPointHasCorrectType(Chip chip)
    {
        if (chip == _enterPoint)
        {
            ChipType enterPointTypes = ChipType.Horizontal | ChipType.LeftDown | ChipType.LeftUp;
            return (chip.CurrentChipType & enterPointTypes) != ChipType.None;
        }
        
        if (chip == _exitPoint)
        {
            ChipType exitPointTypes = ChipType.Horizontal | ChipType.RightDown | ChipType.UpRight;
            return (chip.CurrentChipType & exitPointTypes) != ChipType.None;
        }
        
        return false;
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
                Vector3 pos = new Vector3(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);
                Chip chip = Instantiate(originalEmptyChip, transform, true);
                chip.Init(new ChipPoint(col, row));
                chip.SetClickListener(OnChipClick);
                chip.transform.position = pos;
                chip.SetChipColor(Color.gray);
                var id = field[(rows - 1) - row, col];
                var type = GetTypeById(id);
                chip.SetChipData(type, _bpManager.GetChipSpriteByType(type));
                chips[row, col] = chip;
                
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
    }

    private void OnChipClick(Chip chip)
    {
        OnChipClicked?.Invoke(chip);
    }
    
    private ChipType GetTypeById(int id)
    {
        switch (id)
        {
            case 0:
            case 1:
            case 3: return ChipType.Empty;
            case 2: return ChipType.Block;
            default: throw new Exception($"ChipType by id = {id} not found");
        }
    }
}