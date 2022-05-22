using System;
using Enum;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class LevelEditorCell : Chip
{
    private Button currentButton;
    private Action<LevelEditorCell> onCellClicked;
    private Image image;
    public RectTransform RectTransform => (RectTransform) transform;
    public int Index { get; set; }
    
    
    private void Awake()
    {
        currentButton = GetComponent<Button>();
        image = GetComponent<Image>();
        currentButton.onClick.AddListener(OnCellClick);
    }

    public void SetClickListener(Action<LevelEditorCell> listener)
    {
        onCellClicked = listener;
    }

    private void OnCellClick()
    {
        onCellClicked?.Invoke(this);
    }

    public override void SetChipData(CellType cellType, Sprite sprite)
    {
        image.sprite = sprite;
        CellType = cellType;
    }

    public override void SetChipOnWay(bool isOnWay)
    {
        IsOnWay = isOnWay;
    }
}
