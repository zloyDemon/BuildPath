using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtonsContainer : MonoBehaviour
{
    [SerializeField] private ChipControlButton _originalButtonPrefab;
    [SerializeField] private GridLayoutGroup _gridLayout;
    
    private void Awake()
    {
        foreach (var value in Enum.GetValues(typeof(ChipType)))
        {
            var type = (ChipType) value;
            if (type == ChipType.None || type == ChipType.Block)
                continue;

            ChipControlButton newButton = Instantiate(_originalButtonPrefab, _gridLayout.transform);
            newButton.Init(type);
            newButton.SetClickListener(OnControlButtonClicked);
        }
    }

    private void OnControlButtonClicked(ChipType type)
    {
        BPManager.Instance.ChoseTypeByControl(type);
    }
}
