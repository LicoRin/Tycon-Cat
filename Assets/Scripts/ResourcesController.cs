using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

// ����� ��� �������� ���������� � �������
[System.Serializable]
public class ResourceInfo
{
    public string name;
    public GameObject triggerObject;
    // public ResourceData resourceData; // �������
    public InventoryItemInfo inventoryItemInfo;
    public Text amountText; // ������ ���������� ���� ��� UI
    public int amount; // ��������� ���� ��� �������� ����������
}

public class ResourcesController : MonoBehaviour
{
    public ResourceLevel resLev;
    public List<ResourceInfo> resources;
    public AddingItems inventoryManager;

    void Start()
    {
        // ������������� ��������, ���� ��� �� ������ � ����������
        if (resources == null || resources.Count == 0)
        {
            resources = new List<ResourceInfo>();
            Debug.LogWarning("������� �� ������ � ����������. ����������, �������� �������.");
        }
        // �������� ������� InventoryManager
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<AddingItems>();
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager �� ������ � �����!");
            }
        }
       
    }

    // ����� ��� ���������� ���������� ������� � ���������� � ���������
    public void CollectResource(string resourceName, int value = 1)
    {
        var res = resources.Find(r => r.name == resourceName);
        if (res != null)
        {
            res.amount += value; // ��������� ���������� � ���� ������
            UpdateResourceText(res);

            if (res.inventoryItemInfo != null && inventoryManager != null)
            {
                inventoryManager.AddItemToInventory(res.inventoryItemInfo);
            }
        }
    }

    void UpdateResourceText(ResourceInfo res)
    {
        // ���������, ��� resLev �� null
       
        // ��������� ����� ������ ���� amountText ��������
        if (res.amountText != null)
        {
            res.amountText.text = $"{res.amount}";
            Debug.Log($"������� ����� ��� {res.name}: {res.amountText.text}");
        }
        else
        {
            Debug.LogWarning($"amountText �� �������� ��� �������: {res.name}");
        }
    }

    void Update()
    {
        // �������������� �������� amount � resLev.totalAmount
      
            foreach (var res in resources)
            {
                UpdateResourceText(res);
            }
        
    }
}


