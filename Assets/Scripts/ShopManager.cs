using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class houseManger
{
    public string houseName;
    public GameObject housePrefab;
    public Button houseButton;
    public int housePrice;
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager current;
    
    public bool playerMoved = false;
    

    public List<houseManger> houses = new List<houseManger>();

    void Start()
    {
        for (int i = 0; i < houses.Count; i++)
        {
            int index = i; // Локальная копия для замыкания

            houses[i].houseButton.onClick.AddListener(() => OnHouseButtonClicked(index));
        }
    }
    void OnHouseButtonClicked(int index)
    {
        if (index >= 0 && index < houses.Count)
        {
            GameObject houseToBuy = houses[index].housePrefab;
            BuyHouse(houseToBuy);
            GridBuildingSystem.current.FollowBuilding();
        }
    }


    void BuyHouse(GameObject housePrefab)
    {
        GloballController.current.shopPanel.SetActive(false);
        GloballController.current.shopButton.SetActive(false);
        GridBuildingSystem.current.InitializeWithBuilding(housePrefab);


    }





    
}

