using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    // Drag and drop
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (transform.childCount == 0)
        {
            droppedItem.parentAfterDrag = transform;
        }
        else
        {
            // Optional: swap logic here
            Transform existingItem = transform.GetChild(0);
            existingItem.SetParent(droppedItem.parentAfterDrag); // Move existing item to where dropped item came from
            droppedItem.parentAfterDrag = transform;
        }
    }
}
