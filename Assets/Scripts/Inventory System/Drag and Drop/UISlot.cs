using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    private bool isDraggingItem = false;
    private UIItem currentItem;

    
    public bool HasItem()
    {
        return currentItem != null;
    }

    public bool CanDropInSlot(InventoryItemInfo newItemInfo)
    {
        return true;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        UIItem draggedItem = eventData.pointerDrag.GetComponent<UIItem>();
        UISlot targetSlot = eventData.pointerEnter?.GetComponent<UISlot>();

        if (draggedItem != null && targetSlot != null)
        {
            if (targetSlot.HasItem())
            {
                UIItem targetItem = targetSlot.GetItem();
                UISlot draggedItemSlot = draggedItem.GetLastSlot();

               
                if (draggedItemSlot != null)
                {
                    draggedItemSlot.AddUIItem(targetItem);
                }

                targetSlot.AddUIItem(draggedItem);
            }
            else
            {
               
                targetSlot.AddUIItem(draggedItem);
            }
        }
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDraggingItem = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDraggingItem = false;
        }
    }

    public void AddUIItem(UIItem uiItem)
    {
        uiItem.transform.SetParent(transform);
        uiItem.transform.localPosition = Vector2.zero; // ����� �������
    }

    public void RemoveItem(UIItem uiItem)
    {
        currentItem = null;
        uiItem.transform.SetParent(null); 
    }

    public UIItem GetItem()
    {
        return currentItem;
    }
}
