using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour {

    [SerializeField]
    public ChipType chipType;

    [SerializeField]
    public BPManager manager;
	
    private void OnMouseDown()
    {
        manager.ClickOnControl(chipType);
    }
}
