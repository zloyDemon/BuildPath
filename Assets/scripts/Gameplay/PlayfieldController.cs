using System;
using System.Collections.Generic;
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

    private GameProcessManager gameProcessManager;
    
    private Chip _enterPoint;
    private Chip _exitPoint;
    private Vector2 cellSize;
    private Vector2 cellScale;
    private Chip[,] chips;
    private Dictionary<ChipType, List<ChipSideCheck>> sideChecks;
    private Queue<Chip> rightChips = new Queue<Chip>();
    private Chip _currentChosenChip;


    public Chip[,] Chips => chips;
    public Chip EnterPoint => _enterPoint;
    public Chip ExitPoint => _exitPoint;
    public IEnumerable<Chip> RightChips => rightChips;
    
    public event Action OnCheckCompleted;

    public void Init(GameProcessManager manager)
    {
        gameProcessManager = manager;
        BuildChipGrid();
        InitSides();
    }
    
    public bool IsEnterOrExitPointHasCorrectType(Chip chip)
    {
        ChipType result = ChipType.None;
        
        if (chip == _enterPoint)
        {
            result = ChipType.Horizontal | ChipType.LeftDown | ChipType.LeftUp;
        }
        
        if (chip == _exitPoint)
        {
            result = ChipType.Horizontal | ChipType.RightDown | ChipType.UpRight;
        }
        
        return (chip.CurrentChipType & result) != ChipType.None;
    }
    
    public Sprite GetChipSpriteByType(ChipType type)
    {
        return GetChipSpriteByName(type.ToString());
    }
    
    public Sprite GetChipSpriteByName(string name)
    {
        return _chipsSpriteHolder.GetSpriteByName(name);
    }
    
    public void ChoseTypeByControl(ChipType type)
    {
        if (_currentChosenChip != null)
        {
            _currentChosenChip.SetChipData(type, GetChipSpriteByType(type));
            
            foreach (var chip in Chips)
                chip.SetChipOnWay(false);
            
            CheckGame();
        }
    }
    
    private Chip Check(Chip chip)
    {
        if (!sideChecks.ContainsKey(chip.CurrentChipType))
            return null;

        foreach (var chipSideCheck in sideChecks[chip.CurrentChipType])
        {
            var c = chipSideCheck.Check(chip);
            if (c != null)
            {
                return c;
            }
        }

        return null;
    }
    
    public void CheckGame()
    {
        var playfieldController = gameProcessManager.PlayfieldController;
        Chip chip = GetChipByChipPoint(playfieldController.EnterPoint.ChipPoint);
        
        if (chip.CurrentChipType == ChipType.Empty)
            return;

        rightChips.Clear();
        
        while (chip != null)
        {
            if (chip) rightChips.Enqueue(chip);
            if (chip == playfieldController.EnterPoint || chip == playfieldController.ExitPoint)
            {
                if (!playfieldController.IsEnterOrExitPointHasCorrectType(chip))
                    return;
            }
            
            chip.SetChipOnWay(true);
            
            if (chip == playfieldController.ExitPoint)
            {
                
                Debug.Log("Win");
                OnCheckCompleted?.Invoke();
                return;
            }
            
            chip = Check(chip);
            
        }
        
        OnCheckCompleted?.Invoke();
    }
    
    public Chip GetChipByChipPoint(ChipPoint point)
    {
        return Chips[point.y, point.x];
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
                chip.SetChipData(type, GetChipSpriteByType(type));
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
        if (chip.CurrentChipType == ChipType.Block)
            return;
        
        if (_currentChosenChip != null)
            _currentChosenChip.SetSelect(false);

        _currentChosenChip = chip;
        _currentChosenChip.SetSelect(true);
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
    
    private void InitSides()
    {
        sideChecks = new Dictionary<ChipType, List<ChipSideCheck>>
        {
            {
                ChipType.Horizontal, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.ChipPoint.x != 0,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x - 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.RightDown,
                            ChipType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.x != gameProcessManager.PlayfieldController.Chips.GetLength(0) - 1,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x + 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.LeftDown,
                            ChipType.LeftUp,
                        })
                }
            },
            
            {
                ChipType.Vertical, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.ChipPoint.y != 0,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
                            c.ChipPoint.y - 1)),
                        new List<ChipType>
                        {
                            ChipType.UpRight,
                            ChipType.LeftUp,
                            ChipType.Vertical,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.y != gameProcessManager.PlayfieldController.Chips.GetLength(1) - 1,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
                            c.ChipPoint.y + 1)),
                        new List<ChipType>
                        {
                            ChipType.Vertical,
                            ChipType.LeftDown,
                            ChipType.RightDown,
                        })
                }
            },

            {
                ChipType.LeftUp, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.ChipPoint.x != 0,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x - 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.RightDown,
                            ChipType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.y != gameProcessManager.PlayfieldController.Chips.GetLength(1) - 1,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
                            c.ChipPoint.y + 1)),
                        new List<ChipType>
                        {
                            ChipType.Vertical,
                            ChipType.RightDown,
                            ChipType.LeftDown,
                        })
                }
            },
            
            {
                ChipType.LeftDown, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.ChipPoint.x != 0,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x - 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.RightDown,
                            ChipType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.y != 0,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
                            c.ChipPoint.y - 1)),
                        new List<ChipType>
                        {
                            ChipType.Vertical,
                            ChipType.UpRight,
                            ChipType.LeftUp,
                        })
                }
            },
            
            {
                ChipType.UpRight, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.ChipPoint.y != gameProcessManager.PlayfieldController.Chips.GetLength(1) - 1,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
                            c.ChipPoint.y + 1)),
                        new List<ChipType>
                        {
                            ChipType.Vertical,
                            ChipType.RightDown,
                            ChipType.LeftDown,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.x != gameProcessManager.PlayfieldController.Chips.GetLength(0) - 1,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x + 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.LeftDown,
                            ChipType.LeftUp,
                        })
                }
            },
            
            {
                ChipType.RightDown, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.ChipPoint.x !=gameProcessManager.PlayfieldController.Chips.GetLength(0) - 1,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x + 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.LeftUp,
                            ChipType.LeftDown,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.y != 0,
                        c => GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
                            c.ChipPoint.y - 1)),
                        new List<ChipType>
                        {
                            ChipType.Vertical,
                            ChipType.UpRight,
                            ChipType.LeftUp,
                        })
                }
            },
        };
    }
}