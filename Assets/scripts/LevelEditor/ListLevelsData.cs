using UnityEngine;
using UnityEngine.UI;

public class ListLevelsData : MonoBehaviour
{
    [SerializeField] private ListLevelItem originalListItemPrefab;
    [SerializeField] private Button addListItemButton;
    [SerializeField] private Button removeListItemButton;
    [SerializeField] private RectTransform content;

    private ListLevelItem currentListItem;
    
    
    private void Awake()
    {
        addListItemButton.onClick.AddListener(AddItem);
        removeListItemButton.onClick.AddListener(RemoveItem);
    }

    private void AddItem()
    {
        var listItem = Instantiate(originalListItemPrefab, content, true);
        listItem.OnItemClick += OnItemClick;
        ChangeCurrentItem(listItem);
    }

    private void RemoveItem()
    {
        if (currentListItem != null)
        {
            currentListItem.OnItemClick -= OnItemClick;
            Destroy(currentListItem.gameObject);    
        }
    }

    private void OnItemClick(ListLevelItem item)
    {
        ChangeCurrentItem(item);
    }

    private void ChangeCurrentItem(ListLevelItem item)
    {
        if (currentListItem != null)
        {
            currentListItem.DeselectItem();
        }
        
        currentListItem = item;
        currentListItem.SelectListItem();
    }
}
