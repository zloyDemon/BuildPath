using System;
using Enum;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChipControlButton : MonoBehaviour
{
    [SerializeField] private Image _chipImg;
    [SerializeField] private Button _button;

    private CellType cellType;
    private Action<CellType> OnControlChipTypeClick;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        OnControlChipTypeClick = null;
    }

    public void Init(CellType type, Sprite sprite)
    {
        cellType = type;
        _chipImg.sprite = sprite;
    }

    public void SetClickListener(Action<CellType> clickListener)
    {
        OnControlChipTypeClick = clickListener;
    }

    private void OnButtonClick()
    {
        OnControlChipTypeClick?.Invoke(cellType);
    }
}
