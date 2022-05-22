using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListLevelItem : MonoBehaviour
{
    public struct ListLevelItemData
    {
        public string mainText;

        public ListLevelItemData(string mainText)
        {
            this.mainText = mainText;
        }
    }
    
    [SerializeField] private TextMeshProUGUI listText;
    [SerializeField] private Button itemButton;
    [SerializeField] private Image itemImage;

    public event Action<ListLevelItem> OnItemClick;

    private void Awake()
    {
        itemButton.onClick.AddListener(ClickOnItem);
    }

    public void Init(ListLevelItemData levelItemData)
    {
        listText.text = levelItemData.mainText;
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
