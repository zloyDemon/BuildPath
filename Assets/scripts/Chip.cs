using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{

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
    private ChipPoint chipPoint;
    private SpriteRenderer render;

    public SceneController controller { get; set; }
    public ChipType chip_type { get; set; }

    private void Start()
    {
        render = GetComponent<SpriteRenderer>() as SpriteRenderer;
    }

    public void SetChip(Sprite image, ChipType type)
    {
        render.sprite = image;
        if (type == ChipType.BLOCK)
        {
            isCanClick = false;
        }
        this.chip_type = type;
    }

    public void SetChipPoint(ChipPoint point)
    {
        chipPoint = point;
    }

    private void OnMouseDown()
    {
        OnChipClick();
    }

    public void ActivateChosenChip()
    {
        render.color = Color.red;
    }

    public void DeactivateChosenChip()
    {
        render.color = Color.white;
    }

    private void OnChipClick()
    {
        Debug.Log(chipPoint.x + " " + chipPoint.y);
        controller.ChooseChip(this);
        ActivateChosenChip();
    }
}
