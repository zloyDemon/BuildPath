using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSideCheck
{
    private Chip _currentChip;
    private Predicate<Chip> _sideClampPredicate;
    private Func<Chip, Chip> _sideChipPoint;
    private List<ChipType> _availableTypesOnSide;

    public ChipSideCheck(Predicate<Chip> sideClampPredicate, Func<Chip, Chip> sideChipPoint, List<ChipType> availableTypesOnSide)
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

            if (chip.CurrentChipType != ChipType.Empty && !chip.IsOnWay)
            {
                if (_availableTypesOnSide.Contains(chip.CurrentChipType))
                    result = chip;
            }
        }

        return result;
    }
}
