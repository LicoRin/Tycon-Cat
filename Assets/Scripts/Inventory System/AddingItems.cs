using UnityEngine;

public class AddingItems : UISlot
{
    public InventoryController inventoryController;

    public void AddItemToInventory(InventoryItemInfo itemInfo)
    {
        // Используем метод InventoryController напрямую
        UISlot freeSlot = inventoryController.GetFreeSlot();

        if (freeSlot != null)
        {
            // Создаем префаб UIItem и добавляем его в инвентарь
            GameObject uiItemObject = Instantiate(inventoryController.SlotPrefab); // Замените на ваш фактический префаб
            UIItem uiItem = uiItemObject.GetComponent<UIItem>();

            // Присваиваем информацию о предмете
            uiItem.SetItemInfo(itemInfo);

            // Размещаем UIItem в слоте инвентаря
            freeSlot.AddUIItem(uiItem);

            // Уничтожаем объект предмета
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Не удалось добавить предмет в инвентарь. Инвентарь полон или есть другие проблемы.");
        }
    }
}
