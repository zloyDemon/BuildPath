using System;
using Enum;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelEditorButtonCell : MonoBehaviour
{
    [SerializeField] private CellType type;

    public CellType CellType => type;

    private Action<CellType> onControlButtonClicked;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void SetClickListener(Action<CellType> listener)
    {
        onControlButtonClicked = listener;
    } 

    private void OnClick()
    {
        onControlButtonClicked?.Invoke(type);
    }
}
