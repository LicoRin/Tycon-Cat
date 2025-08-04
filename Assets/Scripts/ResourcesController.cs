using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

// Класс для хранения информации о ресурсе
[System.Serializable]
public class ResourceInfo
{
    public string name;
    public GameObject triggerObject;

    public InventoryItemInfo inventoryItemInfo;
    public Text amountText; // Просто объявление поля для UI
    public string globalVariableName; // например "wood_amount"


}

public class ResourcesController : MonoBehaviour
{
    public GlobalController controller;
    public ResourceLevel resLev;
    public List<ResourceInfo> resources;
    public AddingItems inventoryManager;
    public string resourceId; // Идентификатор ресурса, если нужен

    void Start()
    {
        // Инициализация ресурсов, если они не заданы в инспекторе
        if (resources == null || resources.Count == 0)
        {
            resources = new List<ResourceInfo>();
            Debug.LogWarning("Ресурсы не заданы в инспекторе. Пожалуйста, добавьте ресурсы.");
        }
        // Проверка наличия InventoryManager
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<AddingItems>();
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager не найден в сцене!");
            }
        }
       
    }
    public void CollectResource(string resourceName, int value)
    {
        // Добавляем к глобальной переменной
        GlobalVariableAccessor.AddGlobalValue(resourceName, value);

        // Обновляем UI
        int newAmount = GlobalVariableAccessor.GetGlobalValue(resourceName);
        var res = resources.Find(r => r.name == resourceName);
        if (res != null && res.amountText != null)
        {
            res.amountText.text = newAmount.ToString();
        }
    }






}


