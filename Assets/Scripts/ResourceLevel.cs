using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class LevelInfo
{
    public int level;                // Уровень ресурса
    public int amountPerCollect;     // Сколько предметов даёт за сбор
    public float collectTime;        // Время сбора (сек)
    public float recoveryTime;       // Время восстановления (сек)
}

public class ResourceLevel : MonoBehaviour
{
    [Header("Level Settings")]
    public List<LevelInfo> levels = new List<LevelInfo>();
    public int currentLevel = 0;

    [Header("Resource Settings")]
    public InventoryItemInfo inventoryItemInfo;  // ScriptableObject
    public Text amountText;                      // UI-текст для отображения количества

    private bool isCollecting = false;
    private bool isRecovering = false;

    // Получить информацию о текущем уровне
    public LevelInfo GetCurrentLevelInfo()
    {
        if (levels.Count == 0) return null;
        return levels[Mathf.Clamp(currentLevel, 0, levels.Count - 1)];
    }

    public void UpgradeLevel()
    {
        if (currentLevel < levels.Count - 1)
            currentLevel++;
    }

    public void ResetLevel()
    {
        currentLevel = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log($"Игрок вошёл в триггер ресурса: {gameObject.name}");
            if (!isCollecting && !isRecovering)
            {
                StartCoroutine(StartCollecting());
            }
        }
    }

    private IEnumerator StartCollecting()
    {
        isCollecting = true;
        LevelInfo info = GetCurrentLevelInfo();
        if (info != null)
        {
            // Имитируем время сбора
            yield return new WaitForSeconds(info.collectTime);

            // ? Увеличиваем количество в ScriptableObject
            inventoryItemInfo.Initialize(
                inventoryItemInfo.title,
                inventoryItemInfo.description,
                inventoryItemInfo.indentificator,
                inventoryItemInfo.spriteIcon,
                true,
                inventoryItemInfo.amount + info.amountPerCollect
            );

            Debug.Log($"{inventoryItemInfo.title} теперь: {inventoryItemInfo.amount}");

            // ? Обновляем UI
            if (amountText != null)
                amountText.text = inventoryItemInfo.amount.ToString();

            // После сбора запускаем восстановление
            StartCoroutine(StartRecovery(info.recoveryTime));
        }
        isCollecting = false;
    }

    private IEnumerator StartRecovery(float recoveryTime)
    {
        isRecovering = true;
        yield return new WaitForSeconds(recoveryTime);
        isRecovering = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Игрок вышел из триггера ресурса");
        }
    }
}
