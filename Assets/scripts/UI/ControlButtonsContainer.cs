using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ControlButtonsContainer : MonoBehaviour
{
    [SerializeField] private ChipControlButton _originalButtonPrefab;
    [SerializeField] private GridLayoutGroup _gridLayout;

    private GameProcessManager gameProcessManager;
    
    private void Awake()
    {
        gameProcessManager = GameProcessManager.Instance;
        foreach (var value in Enum.GetValues(typeof(ChipType)))
        {
            var type = (ChipType) value;
            if (type == ChipType.None || type == ChipType.Block)
                continue;
            
            ChipControlButton newButton = Instantiate(_originalButtonPrefab, _gridLayout.transform);
            newButton.Init(type, gameProcessManager.PlayfieldController.GetChipSpriteByType(type));
            newButton.SetClickListener(OnControlButtonClicked);
        }
    }

    private void OnControlButtonClicked(ChipType type)
    {
        gameProcessManager.PlayfieldController.ChoseTypeByControl(type);
    }
}
