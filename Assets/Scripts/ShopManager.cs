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
    public int housePrice;
}

public class ShopManager : MonoBehaviour
{

    public GameObject shopPanel;
    public GameObject shopButton;
    public bool playerMoved = false;

    public List<houseManger> houses = new List<houseManger>();


    public void BuyHouse()
    {
        shopPanel.SetActive(false);
        shopButton.SetActive(true);
        

        foreach (var house in houses)
        {
            if (house.houseName == "House1") // Example condition
            {
                // ������ ���, �� ���� �� ������ ��� ������������
                GameObject newHouse = Instantiate(house.housePrefab, Vector3.zero, Quaternion.identity);

                // ������� ��� � ������� �������������
                GridBuildingSystem.current.InitializeWithBuilding(newHouse);

                break; // ����� ������ ���, ������ ������ �� ����
            }
        }
    }



    public void CloseShopPanel()
    {
        shopPanel.SetActive(false);
        shopButton.SetActive(true);
    }

    public void OpenShopPanel()
    {
        shopPanel.SetActive(true);
        shopButton.SetActive(false);
    }
}

