using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataListView : MonoBehaviour
{
    [SerializeField] private ListLevelItem originalListItemPrefab;
    [SerializeField] private Button addListItemButton;
    [SerializeField] private Button removeListItemButton;
    [SerializeField] private RectTransform content;

    private ListLevelItem currentListItem;
    private List<ListLevelItem> itemList = new List<ListLevelItem>();

    public IEnumerable<ListLevelItem> ItemList => itemList;
    public ListLevelItem CurrentListItem => currentListItem;

    public event Action<ListLevelItem> OnItemSelected;

    private void Awake()
    {
        addListItemButton.onClick.AddListener(AddItem);
        removeListItemButton.onClick.AddListener(RemoveItem);
    }

    public void UpdateLevelData(LevelDataSerialize data)
    {
        //currentListItem.LevelData = data;
    }

    public void InitList()
    {
        AddItem();
    }

    private void AddItem()
    {
        var listItem = Instantiate(originalListItemPrefab, content, true);
        LevelData levelDataSerialize = new LevelData();
        listItem.LevelData = levelDataSerialize;
        itemList.Add(listItem);
        listItem.OnItemClick += OnItemClick;
        ChangeCurrentItem(listItem);
    }

    private void RemoveItem()
    {
        if (currentListItem != null)
        {
            currentListItem.OnItemClick -= OnItemClick;
            itemList.Remove(currentListItem);
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
        OnItemSelected?.Invoke(currentListItem);
    }
}
