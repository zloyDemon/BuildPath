using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enum;
using UnityEngine;
using Zenject;

public class CellsController
{
    private Dictionary<CellType, List<ChipSideCheck>> sideChecks;
    private Queue<Chip> rightChips = new Queue<Chip>();
    private Chip[,] chips;
    private Chip enterPoint;
    private Chip exitPoint;
    
    public IEnumerable<Chip> RightChips => rightChips;
    public Chip EnterPoint => enterPoint;
    public Chip ExitPoint => exitPoint;

    public event Action<CheckStatus> OnCheckCompleted;

    public enum CheckStatus
    {
        NotSuccess,
        Success,
    }
    
    public CellsController()
    {
        InitCheckRules();
    }

    public void SetCells(Chip[,] chips, Chip enterPoint, Chip exitPoint)
    {
        this.chips = chips;
        this.enterPoint = enterPoint;
        this.exitPoint = exitPoint;
    }

    private Chip Check(Chip chip)
    {
        if (!sideChecks.ContainsKey(chip.CellType))
            return null;

        foreach (var chipSideCheck in sideChecks[chip.CellType])
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
        Chip chip = enterPoint;
        
        if (chip.CellType == CellType.Empty)
            return;

        rightChips.Clear();
        
        while (chip != null)
        {
            if (chip)
            {
                rightChips.Enqueue(chip);
            }
            
            if (chip == enterPoint || chip == exitPoint)
            {
                if (!(IsEnterPointHasCorrectType(chip) || IsExitPointHasCorrectType(chip)))
                {
                    return;
                }
            }

            chip.SetChipOnWay(true);
            
            if (chip == exitPoint)
            {
                OnCheckCompleted?.Invoke(CheckStatus.Success);
                return;
            }
            
            chip = Check(chip);
        }
        
        OnCheckCompleted?.Invoke(CheckStatus.NotSuccess);
    }

    public bool IsEnterPointHasCorrectType(Chip chip)
    {
        CellType result = CellType.Horizontal | CellType.LeftDown | CellType.LeftUp;
        return (chip.CellType & result) != CellType.None;
    }

    public bool IsExitPointHasCorrectType(Chip chip)
    {
        CellType result = CellType.Horizontal | CellType.RightDown | CellType.UpRight;
        return (chip.CellType & result) != CellType.None;
    }

    private Chip GetChipByChipPoint(CellPoint point)
    {
        return chips[point.x, point.y];
    }
    
    private void InitCheckRules()
    {
        sideChecks = new Dictionary<CellType, List<ChipSideCheck>>
        {
            {
                CellType.Horizontal, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.CellPoint.x != 0,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x - 1,
                            c.CellPoint.y)),
                        new List<CellType>
                        {
                            CellType.Horizontal,
                            CellType.RightDown,
                            CellType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.CellPoint.x != chips.GetLength(0) - 1,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x + 1,
                            c.CellPoint.y)),
                        new List<CellType>
                        {
                            CellType.Horizontal,
                            CellType.LeftDown,
                            CellType.LeftUp,
                        })
                }
            },
            
            {
                CellType.Vertical, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.CellPoint.y != 0,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x,
                            c.CellPoint.y + 1)),
                        new List<CellType>
                        {
                            CellType.UpRight,
                            CellType.LeftUp,
                            CellType.Vertical,
                        }),

                    new ChipSideCheck(
                        c => c.CellPoint.y != chips.GetLength(1) - 1,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x,
                            c.CellPoint.y - 1)),
                        new List<CellType>
                        {
                            CellType.Vertical,
                            CellType.LeftDown,
                            CellType.RightDown,
                        })
                }
            },

            {
                CellType.LeftUp, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.CellPoint.x != 0,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x - 1,
                            c.CellPoint.y)),
                        new List<CellType>
                        {
                            CellType.Horizontal,
                            CellType.RightDown,
                            CellType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.CellPoint.y != 0,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x,
                            c.CellPoint.y - 1)),
                        new List<CellType>
                        {
                            CellType.Vertical,
                            CellType.RightDown,
                            CellType.LeftDown,
                        })
                }
            },
            
            {
                CellType.LeftDown, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.CellPoint.x != 0,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x - 1,
                            c.CellPoint.y)),
                        new List<CellType>
                        {
                            CellType.Horizontal,
                            CellType.RightDown,
                            CellType.UpRight,
                        }),

                    new ChipSideCheck(
                        c => c.CellPoint.y != chips.GetLength(1) - 1,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x,
                            c.CellPoint.y + 1)),
                        new List<CellType>
                        {
                            CellType.Vertical,
                            CellType.UpRight,
                            CellType.LeftUp,
                        })
                }
            },
            
            {
                CellType.UpRight, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.CellPoint.y != 0,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x,
                            c.CellPoint.y - 1)),
                        new List<CellType>
                        {
                            CellType.Vertical,
                            CellType.RightDown,
                            CellType.LeftDown,
                        }),

                    new ChipSideCheck(
                        c => c.CellPoint.x != chips.GetLength(0) - 1,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x + 1,
                            c.CellPoint.y)),
                        new List<CellType>
                        {
                            CellType.Horizontal,
                            CellType.LeftDown,
                            CellType.LeftUp,
                        })
                }
            },
            
            {
                CellType.RightDown, new List<ChipSideCheck>
                {
                    new ChipSideCheck(
                        c => c.CellPoint.x != chips.GetLength(0) - 1,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x + 1,
                            c.CellPoint.y)),
                        new List<CellType>
                        {
                            CellType.Horizontal,
                            CellType.LeftUp,
                            CellType.LeftDown,
                        }),

                    new ChipSideCheck(
                        c => c.CellPoint.y != chips.GetLength(1) - 1,
                        c => GetChipByChipPoint(CellPoint.CreatChipPoint(c.CellPoint.x,
                            c.CellPoint.y + 1)),
                        new List<CellType>
                        {
                            CellType.Vertical,
                            CellType.UpRight,
                            CellType.LeftUp,
                        })
                }
            },
        };
    }
}
