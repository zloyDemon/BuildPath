using System;
using UnityEngine;
using UnityEngine.UI;

public class ChipControlButton : MonoBehaviour
{
    [SerializeField] private Image _chipImg;
    [SerializeField] private Button _button;

    private ChipType _chipType;
    private Action<ChipType> OnControlChipTypeClick;
    
    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        OnControlChipTypeClick = null;
    }

    public void Init(ChipType type)
    {
        _chipType = type;
        _chipImg.sprite = BPManager.Instance.GetChipSpriteByType(_chipType);
    }

    public void SetClickListener(Action<ChipType> clickListener)
    {
        OnControlChipTypeClick = clickListener;
    }

    private void OnButtonClick()
    {
        OnControlChipTypeClick?.Invoke(_chipType);
    }
}
