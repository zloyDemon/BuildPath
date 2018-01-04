using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour {

    private ChipType _chip_type;

    public ChipType chip_type
    {
        get
        {
            return _chip_type;
        }

        set
        {
            _chip_type = value;
        }
    }

    public void setChip(Sprite image)
    {
        GetComponent<SpriteRenderer>().sprite = image;
    }
}
