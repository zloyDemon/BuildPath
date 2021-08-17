using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Chip : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private List<ChipType> _supportingChipTypes;
    
    private Action<Chip> clickListener;

    public ChipType CurrentChipType { get; set; }

    public ChipPoint ChipPoint { get; private set; }
    
    public bool IsOnWay { get; private set; }
    
    public void Init(ChipPoint chipPoint)
    {
        ChipPoint = chipPoint;
    }
    
    public void SetChipData(ChipType chipType, Sprite sprite)
    {
        CurrentChipType = chipType;
        _text.text = CurrentChipType.ToString();
        _spriteRenderer.sprite = sprite;
    }
    
    public void SetClickListener(Action<Chip> clickListener)
    {
        this.clickListener = clickListener;
    }
    
    public void SetChipColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void SetSelect(bool select)
    {
        Color result = select ? Color.gray : IsOnWay ? Color.yellow : Color.white;
        SetChipColor(result);
    }

    public void SetChipOnWay(bool isOnWay)
    {
        _spriteRenderer.color = isOnWay ? Color.yellow : Color.white;
        IsOnWay = isOnWay;
    }
    
    private void OnMouseDown()
    {
        clickListener?.Invoke(this);
    }
}
