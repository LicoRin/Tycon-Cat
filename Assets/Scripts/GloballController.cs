using UnityEngine;
using UnityEngine.UI;

public class GloballController : MonoBehaviour
{
    public static GloballController current;

    [Header("UI Панели")]
    public GameObject shopPanel;
    public GameObject shopButton;
    public GameObject HomePanel;

    [Header("ScriptableObject ресурсов")]
    public InventoryItemInfo WoodInfo;
    public InventoryItemInfo StoneInfo;
    public InventoryItemInfo MoneyInfo;

    [Header("UI отображение ресурсов")]
    public Text WoodAmountText;
    public Text StoneAmountText;
    public Text MoneyAmountText;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        UpdateResourceUI();
    }

    /// <summary>
    /// ✅ Метод для обновления UI количества ресурсов
    /// </summary>
    public void UpdateResourceUI()
    {
        if (WoodAmountText != null && WoodInfo != null)
            WoodAmountText.text = WoodInfo.amount.ToString();

        if (StoneAmountText != null && StoneInfo != null)
            StoneAmountText.text = StoneInfo.amount.ToString();

        if (MoneyAmountText != null && MoneyInfo != null)
            MoneyAmountText.text = MoneyInfo.amount.ToString();
    }


    public void SetOnHomePanel()
    {
        HomePanel.SetActive(true);
        shopButton.SetActive(false);
    }

    public void SetOffHomePanel()
    {
        HomePanel.SetActive(false);
        shopButton.SetActive(true);
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

    public void CloseNotEnoughPanel()
    {
        ShopManager.current.notEnoughResourcesPanel.SetActive(false);
        ShopManager.current.exitShopButton.SetActive(true);
    }
}
