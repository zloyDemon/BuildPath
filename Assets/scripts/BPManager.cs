using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class BPManager : MonoBehaviour
{
    [SerializeField] private Chip originalEmptyChip;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private Vector2 gridOffset;
    [SerializeField] public Sprite cellSprite;
    [SerializeField] private SpriteHolder _chipsSpriteHolder;
    
    private Chip currentChosenChip;
    private Vector2 cellSize;
    private Vector2 cellScale;
    private Chip[,] chips;
    private ChipController _chipController;
    private Chip _enterPoint;
    private Chip _exitPoint;
    
    public static BPManager Instance { get; private set; }
    public Chip[,] Chips => chips;


    void Awake()
    {
        Instance = this;
        _chipController = new ChipController(this);
        BuildChipGrid();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public Sprite GetChipSpriteByType(ChipType type)
    {
        return _chipsSpriteHolder.GetSpriteByName(type.ToString());
    }

    private void CheckGame()
    {
        Chip chip = GetChipByChipPoint(_enterPoint.ChipPoint);

        if (chip.CurrentChipType == ChipType.Empty)
            return;

        while (chip != null)
        {
            chip.SetChipOnWay(true);

            if (chip == GetChipByChipPoint(_exitPoint.ChipPoint))
            {
                Debug.Log("Wiiiin");
                return;
            }
            
            chip = _chipController.Check(chip);
        }
    }
    
    

    public void ChoseTypeByControl(ChipType type)
    {
        if (currentChosenChip != null)
        {
            currentChosenChip.SetChipData(type, GetChipSpriteByType(type));
            
            foreach (var chip in chips)
                chip.SetChipOnWay(false);
            
            CheckGame();
        }
    }

    public Chip GetChipByChipPoint(ChipPoint point)
    {
        return chips[point.y, point.x];
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

        //originalEmptyChip.transform.localScale = new Vector3(cellScale.x, cellScale.y, 0);

        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;


        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = new Vector3(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);
                Chip chip = Instantiate(originalEmptyChip);
                chip.Init(new ChipPoint(col, row));
                chip.SetClickListener(OnChipClicked);
                chip.transform.position = pos;
                chip.transform.parent = transform;
                var type = GetTypeById(field[(rows - 1) - row, col]);
                chip.SetChipData(type, GetChipSpriteByType(type));
                chips[row, col] = chip;
            }
        }

        _enterPoint = chips[UnityEngine.Random.Range(0, rows - 1), 0];
        _exitPoint = chips[UnityEngine.Random.Range(0, rows - 1), cols - 1];
        
        _enterPoint.SetChipColor(Color.red);
        _exitPoint.SetChipColor(Color.red);
    }

    private void OnChipClicked(Chip chip)
    {
        if (chip.CurrentChipType == ChipType.Block)
            return;
        
        if (currentChosenChip != null)
            currentChosenChip.SetSelect(false);

        currentChosenChip = chip;
        currentChosenChip.SetSelect(true);
    }

    private ChipType GetTypeById(int id)
    {
        switch (id)
        {
            case 0:
            case 1: return ChipType.Empty;
            case 2: return ChipType.Block;
            default: throw new Exception($"ChipType by id = {id} not found");
        }
    }
}
