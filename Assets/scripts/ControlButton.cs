using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour {


    public ChipType chipType;
    public SceneController controller;
	
    private void OnMouseDown()
    {
        Debug.Log(chipType);
        controller.ClickOnControl(chipType);
    }
}
