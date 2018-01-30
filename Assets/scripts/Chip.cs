using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    private const float DEF_CHIP_ALPHA = 1f;
    private const float N_CHECK_CHIP_ALPHA = 0.5f;

    [System.Serializable]
    public struct ChipPoint
    {
        public int x;
        public int y;

        public ChipPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

   
    private bool isCanClick = true;
    private SpriteRenderer render;

    public ChipPoint chipPoint { get; set; }
    public BPManager manager;
    public ChipType chip_type { get; set; }
    public bool isCheck { get;  set; }
    public bool isEnterPoint { get; set; }


    private void Awake()
    {
        chip_type = ChipType.EMPTY;
        render = GetComponent<SpriteRenderer>() as SpriteRenderer;
    }

    private void Start()
    {   
        isCheck = isEnterPoint;
        ActivateChosenChip(isEnterPoint);
    }

    public void SetChip(Sprite image,ChipType type)
    {
        chip_type = type;
        render.sprite = image;
    }

    public void SetChipPoint(ChipPoint point)
    {
        chipPoint = point;
    }

    private void OnMouseDown()
    {
        OnChipClick();
    }

    public void ActivateChosenChip(bool active)
    {
        float alpha = render.color.a;
        Color color = render.color;
        color = active ? Color.red : Color.white;
        color.a = alpha;
        render.color = color;
    }

    private void OnChipClick()
    {
        if(chip_type != ChipType.BLOCK)
        {
            Debug.Log(chipPoint.x + " " + chipPoint.y + " type " + chip_type + " " + isCheck);
            manager.ChooseChip(this);
        }     
    }

    private Color SetAlpha(float alpha)
    {
        if(chip_type!= ChipType.EMPTY)
        {
            Color color = render.color;
            color.a = alpha;
            return color;
        }
        return Color.white;
    }

    public void SetChipCheck(bool is_check)
    {
        if(chip_type != ChipType.EMPTY && chip_type != ChipType.BLOCK && !isEnterPoint)
        {
            isCheck = is_check;
            render.color = SetAlpha(isCheck ? DEF_CHIP_ALPHA : N_CHECK_CHIP_ALPHA);
        }
    }
}
