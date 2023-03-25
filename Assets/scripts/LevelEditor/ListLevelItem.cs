using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListLevelItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI listText;
    [SerializeField] private Button itemButton;
    [SerializeField] private Image itemImage;

    public LevelData LevelData { get; set; }
    
    public event Action<ListLevelItem> OnItemClick;

    private void Awake()
    {
        itemButton.onClick.AddListener(ClickOnItem);
    }

    public void SelectListItem()
    {
        var color = new Color32(255, 255, 255, 125);
        itemImage.color = color;
    }

    public void DeselectItem()
    {
        itemImage.color = Color.clear;
    }

    private void ClickOnItem()
    {
        OnItemClick?.Invoke(this);
    }
}
