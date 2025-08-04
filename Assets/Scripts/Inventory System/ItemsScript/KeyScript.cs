using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool keyState = false;
    public InventoryItemInfo _itemKey;
    public bool isOnInventory = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            keyState = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            keyState = false;
        }
    }

    public void AddKeyToSlot(InventoryController inventoryController)
    {
        if (keyState)
        {
           
            UISlot freeSlot = inventoryController.GetFreeSlot();

            if (freeSlot != null)
            {
              
                GameObject uiItemObject = Instantiate(inventoryController.SlotPrefab); 
                UIItem uiItem = uiItemObject.GetComponent<UIItem>();

              
                uiItem.SetItemInfo(_itemKey);

              
                uiItem.itemInfo = _itemKey;

              
                freeSlot.AddUIItem(uiItem);

              
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Не удалось добавить предмет в инвентарь. Инвентарь полон или есть другие проблемы.");
            }
        }
    }
}
