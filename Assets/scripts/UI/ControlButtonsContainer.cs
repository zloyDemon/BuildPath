using System;
using Enum;
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
        foreach (var value in System.Enum.GetValues(typeof(CellType)))
        {
            var type = (CellType) value;
            if (type == CellType.None || type == CellType.Block)
                continue;
            
            ChipControlButton newButton = Instantiate(_originalButtonPrefab, _gridLayout.transform);
            newButton.Init(type, gameProcessManager.PlayfieldController.GetChipSpriteByType(type));
            newButton.SetClickListener(OnControlButtonClicked);
        }
    }

    private void OnControlButtonClicked(CellType type)
    {
        gameProcessManager.PlayfieldController.ChoseTypeByControl(type);
    }
}
