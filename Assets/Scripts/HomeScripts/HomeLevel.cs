using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HomeLevelInfo
{
    public int level;
    public GameObject homePrefab;
    public int upgradePrice;
    public Sprite HomeImage;
}

public class HomeLevel : MonoBehaviour
{
    [Header("Level Settings")]
    public List<HomeLevelInfo> levels = new List<HomeLevelInfo>();
    [Range(0, 100)]
    public int currentLevel = 0;

    [Header("Home Instance")]
    public Transform homeParent; // Родитель для дома
    private GameObject currentHomeInstance;

    private int previousLevel = -1;

    private void Start()
    {
        UpdateHomeVisual(); // При запуске загружаем нужный префаб
    }

    private void Update()
    {
        if (currentLevel != previousLevel)
        {
            previousLevel = currentLevel; // Сначала обновляем
            UpdateHomeVisual();           // Потом визуально обновляем
        }
    }


    private void UpdateHomeVisual()
    {
        if (currentLevel < 0 || currentLevel >= levels.Count)
        {
            Debug.LogWarning("Неверный уровень дома: " + currentLevel);
            return;
        }

        // Удалить старый
        if (currentHomeInstance != null)
        {
            Destroy(currentHomeInstance);
        }

        // Создать новый
        GameObject prefab = levels[currentLevel].homePrefab;
        currentHomeInstance = Instantiate(prefab, homeParent);

        // Сбросить локальную позицию и поворот
        currentHomeInstance.transform.localPosition = Vector3.zero;
        currentHomeInstance.transform.localRotation = Quaternion.identity;

        previousLevel = currentLevel;

        // ?? Вызов перестройки NavMesh после смены уровня
        UpdateArea();
    }

    private void UpdateArea()
    {
        var navMeshBaker = FindObjectOfType<RuntimeNavMeshBaker>();
        if (navMeshBaker != null)
        {
            navMeshBaker.BakeNavMesh();
        }
    }

   
}
