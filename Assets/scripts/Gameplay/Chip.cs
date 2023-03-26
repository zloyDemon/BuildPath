using System;
using System.Collections.Generic;
using Enum;
using TMPro;
using UnityEngine;


public class Chip : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 p;
    private Action<Chip> clickListener;
    public CellType CellType { get; set; }
    public CellPoint CellPoint { get; set; }
    public bool IsOnWay { get; set; }
    
    public void Init(CellPoint cellPoint)
    {
        CellPoint = cellPoint;
        SetP();
    }

    public void SetP()
    {
        p = new Vector2(CellPoint.row, CellPoint.column);
    }
    
    public virtual void SetChipData(CellType cellType, Sprite sprite)
    {
        CellType = cellType;
        spriteRenderer.sprite = sprite;
    }
    
    public void SetClickListener(Action<Chip> clickListener)
    {
        this.clickListener = clickListener;
    }
    
    public void SetChipColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void SetSelect(bool select)
    {
        //SetChipColor(select ? Color.white : Color.grey);
        transform.localScale = Vector3.one * (select ? 0.6f : 0.5f);
    }

    public virtual void SetChipOnWay(bool isOnWay)
    {
        SetChipColor(isOnWay ? Color.white : Color.gray);
        IsOnWay = isOnWay;
    }
    
    private void OnMouseDown()
    {
        clickListener?.Invoke(this);
    }
}
