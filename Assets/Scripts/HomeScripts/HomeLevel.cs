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
    public Transform homeParent; 
    private GameObject currentHomeInstance;

    private int previousLevel = -1;

    private void Start()
    {
        UpdateHomeVisual(); 
    }

    private void Update()
    {
        if (currentLevel != previousLevel)
        {
            previousLevel = currentLevel; 
            UpdateHomeVisual();          
        }
    }


    private void UpdateHomeVisual()
    {
        if (currentLevel < 0 || currentLevel >= levels.Count)
        {
            Debug.LogWarning("�������� ������� ����: " + currentLevel);
            return;
        }

      
        if (currentHomeInstance != null)
        {
            Destroy(currentHomeInstance);
        }

       
        GameObject prefab = levels[currentLevel].homePrefab;
        currentHomeInstance = Instantiate(prefab, homeParent);

       
        currentHomeInstance.transform.localPosition = Vector3.zero;
        currentHomeInstance.transform.localRotation = Quaternion.identity;

        previousLevel = currentLevel;

        
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
