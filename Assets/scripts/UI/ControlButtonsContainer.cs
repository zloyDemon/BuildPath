using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ControlButtonsContainer : MonoBehaviour
{
    [SerializeField] private ChipControlButton _originalButtonPrefab;
    [SerializeField] private GridLayoutGroup _gridLayout;

    private BPManager _bpManager;
    
    private void Awake()
    {
        _bpManager = BPManager.Instance;
        foreach (var value in Enum.GetValues(typeof(ChipType)))
        {
            var type = (ChipType) value;
            if (type == ChipType.None || type == ChipType.Block)
                continue;
            
            ChipControlButton newButton = Instantiate(_originalButtonPrefab, _gridLayout.transform);
            newButton.Init(type, _bpManager.GetChipSpriteByType(type));
            newButton.SetClickListener(OnControlButtonClicked);
        }
    }

    private void OnControlButtonClicked(ChipType type)
    {
        _bpManager.ChoseTypeByControl(type);
    }
}
