using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class houseManger
{
    public string houseName;
    public GameObject housePrefab;
    public GameObject blockOverlay; // объект, который накрывает кнопку (в инспекторе)
    public Button houseButton;

    public Text HousePriceText;
    public int housePrice;

    public Text HouseAmountText;
    public int amountOfHouse;

    public Text workersNameText;
    public string workerName;

    public GameObject workerPrefab;
    public InventoryItemInfo inventoryItemInfo;

    public void UpdateUI()
    {
        // Показываем сколько домов можно купить
        if (HouseAmountText != null)
            HouseAmountText.text = amountOfHouse.ToString();

        if (HousePriceText != null)
            HousePriceText.text = $"{housePrice}$";

        if (workersNameText != null)
            workersNameText.text = workerName;

        if (houseButton != null)
        {
            // Кнопка активна, если лимит домов больше 0 (даже если ресурсов не хватает — кнопка не блокируется)
            houseButton.interactable = amountOfHouse > 0;

            // Показываем overlay, если лимит исчерпан (0)
            if (blockOverlay != null)
                blockOverlay.SetActive(amountOfHouse == 0);

            var btnText = houseButton.GetComponentInChildren<Text>();
            if (btnText != null)
                btnText.text = "BUY";
        }
    }

}


public class ShopManager : MonoBehaviour
{
    public static ShopManager current;

    [Header("Панели страниц")]
    public List<GameObject> pages = new List<GameObject>(); // ← твои 3 панели
    private int currentPage = 0;

    [Header("Кнопки переключения")]
    public Button nextPageButton;
    public Button prevPageButton;

    [Header("Остальное")]
    public GameObject exitShopButton;
    public List<houseManger> houses = new List<houseManger>();

    [Header("UI для ошибок")]
    public GameObject notEnoughResourcesPanel;
    public Text notEnoughResourcesText;


    void Awake()
    {
        current = this;
    }

    void Start()
    {
        // Назначение кнопок переключения
        nextPageButton.onClick.AddListener(NextPage);
        prevPageButton.onClick.AddListener(PreviousPage);

        // Назначение кнопок покупки
        for (int i = 0; i < houses.Count; i++)
        {
            int index = i;
            houses[i].houseButton.onClick.AddListener(() => OnHouseButtonClicked(index));
        }

        ShowPage(currentPage);
    }

    void ShowPage(int pageIndex)
    {
        currentPage = Mathf.Clamp(pageIndex, 0, pages.Count - 1);

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        prevPageButton.gameObject.SetActive(currentPage > 0);
        nextPageButton.gameObject.SetActive(currentPage < pages.Count - 1);
    }

    void NextPage()
    {
        ShowPage(currentPage + 1);
    }

    void PreviousPage()
    {
        ShowPage(currentPage - 1);
    }

    void OnHouseButtonClicked(int index)
    {
        if (index < 0 || index >= houses.Count)
            return;

        var selectedHouse = houses[index];
        var item = selectedHouse.inventoryItemInfo;

        if (item == null)
        {
            Debug.LogError("InventoryItemInfo не назначен на объекте дома!");
            return;
        }

        if (selectedHouse.amountOfHouse <= 0)
        {
            // Уже нельзя покупать — лимит исчерпан
            Debug.Log("Лимит домов исчерпан, купить нельзя.");
            return;
        }

        if (item.amount >= selectedHouse.housePrice)
        {
            BuyHouse(selectedHouse);
            GridBuildingSystem.current.FollowBuilding();
        }
        else
        {
            ShowNotEnoughResourcesPanel(selectedHouse.housePrice - item.amount);
        }
    }


    void BuyHouse(houseManger house)
    {
        house.inventoryItemInfo.DecreaseAmount(house.housePrice);
        house.amountOfHouse--; // уменьшаем лимит по доступным домам

        GloballController.current.shopPanel.SetActive(false);
        GloballController.current.shopButton.SetActive(false);

        GridBuildingSystem.current.InitializeWithBuilding(house.housePrefab);

        DisplayHouseInfo();
    }


    private void Update()
    {
       DisplayHouseInfo();
    }
    void DisplayHouseInfo()
    {
        foreach (var house in houses)
        {
            house.UpdateUI();
        }
    }

    void ShowNotEnoughResourcesPanel(int missing)
    {
        if (notEnoughResourcesPanel != null && notEnoughResourcesText != null)
        {
            notEnoughResourcesPanel.SetActive(true);
            notEnoughResourcesText.text = $"Недостаточно ресурсов!\nНе хватает: {missing}";
            exitShopButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Панель или текст для ошибки не назначены в инспекторе!");
        }
    }
}
