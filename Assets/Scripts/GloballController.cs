using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GloballController : MonoBehaviour
{

    public static GloballController current;
    public GameObject shopPanel;
    public GameObject shopButton;
    public GameObject HomePanel;

    public InventoryItemInfo inventoryItemInfo;  // ScriptableObject
    public Text MoneyAmountText;                      // UI-текст дл€ отображени€ количества

    private void Update()
    {
        
        // » сразу обновл€ем UI
        if (MoneyAmountText != null && inventoryItemInfo != null)
        {
            MoneyAmountText.text = inventoryItemInfo.amount.ToString();
        }
    }
    public void Awake()
    {
        current = this;
    }
    public void SetOnHomePanel()
    {
        HomePanel.SetActive(true);
        GloballController.current.shopButton.SetActive(false);
    }
    public void SetOffHomePanel()
    {
        HomePanel.SetActive(false);
        GloballController.current.shopButton.SetActive(true);
    }

    public void CloseShopPanel()
    {
        GloballController.current.shopPanel.SetActive(false);
        GloballController.current.shopButton.SetActive(true);
    }

    public void OpenShopPanel()
    {
        GloballController.current.shopPanel.SetActive(true);
        GloballController.current.shopButton.SetActive(false);
    }
}
