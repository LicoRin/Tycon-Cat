using UnityEngine;

public class AddingItems : UISlot
{
    public InventoryController inventoryController;

    public void AddItemToInventory(InventoryItemInfo itemInfo)
    {
        // ���������� ����� InventoryController ��������
        UISlot freeSlot = inventoryController.GetFreeSlot();

        if (freeSlot != null)
        {
            // ������� ������ UIItem � ��������� ��� � ���������
            GameObject uiItemObject = Instantiate(inventoryController.SlotPrefab); // �������� �� ��� ����������� ������
            UIItem uiItem = uiItemObject.GetComponent<UIItem>();

            // ����������� ���������� � ��������
            uiItem.SetItemInfo(itemInfo);

            // ��������� UIItem � ����� ���������
            freeSlot.AddUIItem(uiItem);

            // ���������� ������ ��������
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("�� ������� �������� ������� � ���������. ��������� ����� ��� ���� ������ ��������.");
        }
    }
}
