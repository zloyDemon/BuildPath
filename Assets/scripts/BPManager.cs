using TMPro;
using UnityEngine;

public class BPManager : MonoBehaviour
{
    [SerializeField] private SpriteHolder _chipsSpriteHolder;
    [SerializeField] private PlayfieldController _playfieldController;

    private Chip _currentChosenChip;
    private Vector2 cellSize;
    private Vector2 cellScale;
    private ChipController _chipController;

    public PlayfieldController PlayfieldController => _playfieldController;
    public ChipController ChipController => _chipController;


    public static BPManager Instance;

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
        _chipController.CheckGame();
    }

    public void ChoseTypeByControl(ChipType type)
    {
        if (_currentChosenChip != null)
        {
            _currentChosenChip.SetChipData(type, GetChipSpriteByType(type));
            
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
        
        if (_currentChosenChip != null)
            _currentChosenChip.SetSelect(false);

        _currentChosenChip = chip;
        _currentChosenChip.SetSelect(true);
    }
}
