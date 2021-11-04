using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChipController
{
    private Dictionary<ChipType, List<ChipSideCheck>> sideChecks;
    private BPManager _bpManager;
    private Queue<Chip> rightChips = new Queue<Chip>();

    public IEnumerable<Chip> RightChips => rightChips;

    public event Action OnCheckCompleted;
    
    public ChipController(BPManager manager)
    {
        _bpManager = manager;
        InitSides();
    }

    public Chip Check(Chip chip)
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
        var playfieldController = _bpManager.PlayfieldController;
        Chip chip = _bpManager.GetChipByChipPoint(playfieldController.EnterPoint.ChipPoint);
        
        if (chip.CurrentChipType == ChipType.Empty)
            return;

        rightChips.Clear();
        
        while (chip != null)
        {
            if (chip) rightChips.Enqueue(chip);
            if (chip == playfieldController.EnterPoint || chip == playfieldController.ExitPoint)
            {
                if (!playfieldController.EnterOrExitPointHasCorrectType(chip))
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

    private void InitSides()
    {
        sideChecks = new Dictionary<ChipType, List<ChipSideCheck>>
        {
            {
                ChipType.Horizontal, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.ChipPoint.x != 0,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x - 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.RightDown,
                            ChipType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.x != _bpManager.PlayfieldController.Chips.GetLength(0) - 1,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x + 1,
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
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
                            c.ChipPoint.y - 1)),
                        new List<ChipType>
                        {
                            ChipType.UpRight,
                            ChipType.LeftUp,
                            ChipType.Vertical,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.y != _bpManager.PlayfieldController.Chips.GetLength(1) - 1,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
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
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x - 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.RightDown,
                            ChipType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.y != _bpManager.PlayfieldController.Chips.GetLength(1) - 1,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
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
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x - 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.RightDown,
                            ChipType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.y != 0,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
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
                        c => c.ChipPoint.y != _bpManager.PlayfieldController.Chips.GetLength(1) - 1,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
                            c.ChipPoint.y + 1)),
                        new List<ChipType>
                        {
                            ChipType.Vertical,
                            ChipType.RightDown,
                            ChipType.LeftDown,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.x != _bpManager.PlayfieldController.Chips.GetLength(0) - 1,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x + 1,
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
                        c => c.ChipPoint.x !=_bpManager.PlayfieldController.Chips.GetLength(0) - 1,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x + 1,
                            c.ChipPoint.y)),
                        new List<ChipType>
                        {
                            ChipType.Horizontal,
                            ChipType.LeftUp,
                            ChipType.LeftDown,
                        }),

                    new ChipSideCheck(
                        c => c.ChipPoint.y != 0,
                        c => _bpManager.GetChipByChipPoint(ChipPoint.CreatChipPoint(c.ChipPoint.x,
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
