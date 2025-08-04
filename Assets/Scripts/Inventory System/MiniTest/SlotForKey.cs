using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotForKey : MonoBehaviour, IDropHandler
{
    
    private KeyScript keySetup;
    public bool isKeyOnSlot = false;
    [SerializeField] private InventoryItemInfo _key;

    public virtual void OnDrop(PointerEventData eventData)
    {
        UIItem droppedItem = eventData.pointerDrag.GetComponent<UIItem>();

        if (droppedItem != null)
        {
            // Проверяем, является ли предмет ключом по его идентификатору
            if (droppedItem.itemInfo != null && droppedItem.itemInfo.indentificator == "key")
            {
                isKeyOnSlot = true;
            }

            var OtherItemTransform = eventData.pointerDrag.transform;
            OtherItemTransform.SetParent(transform);
            OtherItemTransform.localPosition = Vector2.zero;
        }
    }

   
}
