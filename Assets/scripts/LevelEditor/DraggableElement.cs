using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableElement : MonoBehaviour, IDragHandler
{
    public event Action<PointerEventData> OnDragging;
    
    public void OnDrag(PointerEventData eventData)
    {
        OnDragging?.Invoke(eventData);
    }
}
