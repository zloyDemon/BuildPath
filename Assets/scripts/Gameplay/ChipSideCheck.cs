using System;
using System.Collections.Generic;
using Enum;

public class ChipSideCheck
{
    private Predicate<Chip> _sideClampPredicate;
    private Func<Chip, Chip> _sideChipPoint;
    private List<CellType> _availableTypesOnSide;

    public ChipSideCheck(Predicate<Chip> sideClampPredicate, Func<Chip, Chip> sideChipPoint, List<CellType> availableTypesOnSide)
    {
        _sideClampPredicate = sideClampPredicate;
        _sideChipPoint = sideChipPoint;
        _availableTypesOnSide = availableTypesOnSide;
    }

    public Chip Check(Chip c)
    {
        Chip result = null;
        
        if (_sideClampPredicate(c))
        {
            Chip chip = _sideChipPoint(c);

            if (chip.CellType != CellType.Empty && !chip.IsOnWay)
            {
                if (_availableTypesOnSide.Contains(chip.CellType))
                    result = chip;
            }
        }

        return result;
    }
}
