using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    private const float DEF_CHIP_ALPHA = 1f;
    private const float N_CHECK_CHIP_ALPHA = 0.5f;

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
    public SceneController controller { get; set; }
    public ChipType chip_type { get; set; }
    public bool isCheck { get; set; }
    public bool isEnterPoint { get; set; }


    private void Start()
    {
        chip_type = ChipType.EMPTY;
        render = GetComponent<SpriteRenderer>() as SpriteRenderer;
        isCheck = isEnterPoint;
        ActivateChosenChip(isEnterPoint);
    }

    public void SetChip(Sprite image)
    {  
        if (chip_type == ChipType.BLOCK)
        {
            isCanClick = false;
        }
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
        Debug.Log(chipPoint.x + " " + chipPoint.y + " type " + chip_type + " " + isCheck);
        controller.ChooseChip(this);
        ActivateChosenChip(true);
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
        if(chip_type != ChipType.EMPTY && !isEnterPoint)
        {
            isCheck = is_check;
            render.color = SetAlpha(isCheck ? DEF_CHIP_ALPHA : N_CHECK_CHIP_ALPHA);
        }
    }
}
