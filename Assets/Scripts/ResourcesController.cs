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
    // public ResourceData resourceData; // Удалено
    public InventoryItemInfo inventoryItemInfo;
    public Text amountText; // Просто объявление поля для UI
    public int amount; // Добавлено поле для хранения количества
}

public class ResourcesController : MonoBehaviour
{
    public ResourceLevel resLev;
    public List<ResourceInfo> resources;
    public AddingItems inventoryManager;

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

    // Метод для увеличения количества ресурса и добавления в инвентарь
    public void CollectResource(string resourceName, int value = 1)
    {
        var res = resources.Find(r => r.name == resourceName);
        if (res != null)
        {
            res.amount += value; // Сохраняем количество в поле класса
            UpdateResourceText(res);

            if (res.inventoryItemInfo != null && inventoryManager != null)
            {
                inventoryManager.AddItemToInventory(res.inventoryItemInfo);
            }
        }
    }

    void UpdateResourceText(ResourceInfo res)
    {
        // Проверяем, что resLev не null
       
        // Обновляем текст только если amountText назначен
        if (res.amountText != null)
        {
            res.amountText.text = $"{res.amount}";
            Debug.Log($"Обновлён текст для {res.name}: {res.amountText.text}");
        }
        else
        {
            Debug.LogWarning($"amountText не назначен для ресурса: {res.name}");
        }
    }

    void Update()
    {
        // Синхронизируем значения amount с resLev.totalAmount
      
            foreach (var res in resources)
            {
                UpdateResourceText(res);
            }
        
    }
}


