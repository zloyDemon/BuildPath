using System;
using TMPro;
using UnityEngine;


public class BPManager : MonoBehaviour
{
    [SerializeField] private SpriteHolder _chipsSpriteHolder;
    [SerializeField] private PlayfieldController _playfieldController;

    [SerializeField] private TextMeshProUGUI _timerText;
    
    private Chip currentChosenChip;
    private Vector2 cellSize;
    private Vector2 cellScale;
    private ChipController _chipController;


    public static BPManager Instance { get; private set; }
    public PlayfieldController PlayfieldController => _playfieldController;


    void Awake()
    {
        Instance = this;
        _chipController = new ChipController(this);
        _playfieldController.Init();
        _playfieldController.OnChipClicked += OnChipClicked;
    }

    private void OnDestroy()
    {
        _playfieldController.OnChipClicked -= OnChipClicked;
        Instance = null;
    }

    public Sprite GetChipSpriteByType(ChipType type)
    {
        return _chipsSpriteHolder.GetSpriteByName(type.ToString());
    }

    public Sprite GetChipSpriteByName(string name)
    {
        return _chipsSpriteHolder.GetSpriteByName(name);
    }

    private void CheckGame()
    {
        Chip chip = GetChipByChipPoint(_playfieldController.EnterPoint.ChipPoint);
        
        if (chip.CurrentChipType == ChipType.Empty)
            return;

        while (chip != null)
        {
            if (chip == _playfieldController.EnterPoint || chip == _playfieldController.ExitPoint)
            {
                if (!_playfieldController.EnterOrExitPointHasCorrectType(chip))
                    return;
            }
            
            chip.SetChipOnWay(true);
            
            if (chip == _playfieldController.ExitPoint)
            {
                Debug.Log("Win");
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
            
            foreach (var chip in _playfieldController.Chips)
                chip.SetChipOnWay(false);
            
            CheckGame();
        }
    }

    public Chip GetChipByChipPoint(ChipPoint point)
    {
        return _playfieldController.Chips[point.y, point.x];
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
}
