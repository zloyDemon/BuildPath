using System.Collections.Generic;

public class ChipController
{
    private Dictionary<ChipType, List<ChipSideCheck>> sideChecks;
    private BPManager _bpManager;

    public ChipController(BPManager bpManager)
    {
        _bpManager = bpManager;
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
                        c => c.ChipPoint.y != BPManager.Instance.PlayfieldController.Chips.GetLength(1) - 1,
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
                        c => c.ChipPoint.y != BPManager.Instance.PlayfieldController.Chips.GetLength(1) - 1,
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
