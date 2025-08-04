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

    public InventoryItemInfo inventoryItemInfo;
    public Text amountText; // ������ ���������� ���� ��� UI
    public string globalVariableName; // �������� "wood_amount"


}

public class ResourcesController : MonoBehaviour
{
    public GlobalController controller;
    public ResourceLevel resLev;
    public List<ResourceInfo> resources;
    public AddingItems inventoryManager;
    public string resourceId; // ������������� �������, ���� �����

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
    public void CollectResource(string resourceName, int value)
    {
        // ��������� � ���������� ����������
        GlobalVariableAccessor.AddGlobalValue(resourceName, value);

        // ��������� UI
        int newAmount = GlobalVariableAccessor.GetGlobalValue(resourceName);
        var res = resources.Find(r => r.name == resourceName);
        if (res != null && res.amountText != null)
        {
            res.amountText.text = newAmount.ToString();
        }
    }






}


