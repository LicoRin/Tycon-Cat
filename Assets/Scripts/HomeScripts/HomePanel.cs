using UnityEngine;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    [Header("UI References")]
    public Image currentLevelImage;
    public Text curerntLevelText;
    public Text curerntPriceText;

    [Header("Upgrade Resource")]
    public InventoryItemInfo ValueResource; // ScriptableObject с количеством

    [Header("References")]
    public HomeLevel homeLevel; // Ссылка на HomeLevel

    private void Start()
    {
        UpdateUI(); // Обновить интерфейс при старте
    }

    public void UpgradeHome()
    {
        int level = homeLevel.currentLevel;

        // Проверка: максимальный уровень?
        if (level >= homeLevel.levels.Count - 1)
        {
            Debug.Log("Максимальный уровень достигнут");
            return;
        }

        HomeLevelInfo nextLevelInfo = homeLevel.levels[level + 1];
        int price = nextLevelInfo.upgradePrice;

        // Проверка, хватает ли ресурса
        if (ValueResource.amount >= price)
        {
            // Уменьшаем ресурс
            DecreaseResource(ValueResource, price);

            // Повышаем уровень
            homeLevel.currentLevel++;

            // Обновляем визуал
            UpdateUI();
        }
        else
        {
            Debug.Log("Недостаточно ресурсов для улучшения");
        }
    }

    private void DecreaseResource(InventoryItemInfo resource, int amount)
    {
        var so = resource;
        var newAmount = Mathf.Max(0, so.amount - amount);

        // Важно: так как ScriptableObject сериализован, нужно отразить изменения
#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(so, "Upgrade Resource Used");
        UnityEditor.EditorUtility.SetDirty(so);
#endif
        typeof(InventoryItemInfo)
            .GetField("_amount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(so, newAmount);
    }

    public void UpdateUI()
    {
        int level = homeLevel.currentLevel;
        if (level < 0 || level >= homeLevel.levels.Count) return;

        HomeLevelInfo info = homeLevel.levels[level];

        if (currentLevelImage != null && info.HomeImage != null)
        {
            currentLevelImage.sprite = info.HomeImage;
            currentLevelImage.enabled = true;
        }

        if (curerntLevelText != null)
        {
            curerntLevelText.text = "Level " + info.level;
        }

        if (curerntPriceText != null)
        {
            int nextLevel = level + 1;
            if (nextLevel < homeLevel.levels.Count)
                curerntPriceText.text = "" + homeLevel.levels[nextLevel].upgradePrice;
            else
                curerntPriceText.text = "MAX";
        }
    }

}
