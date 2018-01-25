using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour {

    [SerializeField]
    public ChipType chipType;

    [SerializeField]
    public SceneController controller;
	
    private void OnMouseDown()
    {
        controller.ClickOnControl(chipType);
    }
}
