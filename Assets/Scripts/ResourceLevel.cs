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
    public string resourceName; // Имя ресурса для отладки

    [Header("Resource Settings")]
    public InventoryItemInfo inventoryItemInfo;  // ScriptableObject
    public Text amountText;                      // UI-текст для отображения количества

    [Header("Animation Settings")]
    public Animator animator; // Локальный Animator для объекта

    private bool isPlayerInside = false; // Игрок в триггере
    private bool isRecovering = false;   // Ресурс восстанавливается
    private float collectProgress = 0f;  // Прогресс сбора (сек)

    private LevelInfo CurrentInfo => GetCurrentLevelInfo();

    private void Start()
    {

        
        // При старте ставим Idle
        SetAnimState(true, false, false);

        // И сразу обновляем UI
        if (amountText != null && inventoryItemInfo != null)
        {
            amountText.text = inventoryItemInfo.amount.ToString();
        }
    }

    private void Update()
    {
        if (isPlayerInside && !isRecovering && CurrentInfo != null)
        {
            // Продвигаем прогресс сбора
            collectProgress += Time.deltaTime;

            // Включаем анимацию сбора
            SetAnimState(false, true, false);

            // Проверяем завершение сбора
            if (collectProgress >= CurrentInfo.collectTime)
            {
                CompleteCollection();
            }
        }
        else if (!isRecovering)
        {
            // Игрок не собирает → Idle
            SetAnimState(true, false, false);
        }
    }

    private void CompleteCollection()
    {
        collectProgress = 0f; // Сбрасываем прогресс для следующего сбора

        // ✅ Увеличиваем количество в ScriptableObject
        inventoryItemInfo.Initialize(
            inventoryItemInfo.title,
            inventoryItemInfo.description,
            inventoryItemInfo.indentificator,
            inventoryItemInfo.spriteIcon,
            true,
            inventoryItemInfo.amount + CurrentInfo.amountPerCollect
        );

        Debug.Log($"{inventoryItemInfo.title} теперь: {inventoryItemInfo.amount}");

        // ✅ Обновляем UI
        if (amountText != null)
            amountText.text = inventoryItemInfo.amount.ToString();

        // Переход к восстановлению
        StartCoroutine(StartRecovery(CurrentInfo.recoveryTime));
    }

    private IEnumerator StartRecovery(float recoveryTime)
    {
        isRecovering = true;
        collectProgress = 0f; // Сброс прогресса, чтобы начать заново

        // Анимация восстановления
        SetAnimState(false, false, true);

        yield return new WaitForSeconds(recoveryTime);

        // Возврат в Idle
        SetAnimState(true, false, false);

        isRecovering = false;
    }

    private void SetAnimState(bool idle, bool cutting, bool recovery)
    {
        if (animator == null) return;

        animator.SetBool("Idle", idle);
        animator.SetBool("isCutting", cutting);
        animator.SetBool("isRecovery", recovery);
    }

    private LevelInfo GetCurrentLevelInfo()
    {
        if (levels.Count == 0) return null;
        return levels[Mathf.Clamp(currentLevel, 0, levels.Count - 1)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log($"Игрок вошёл в триггер ресурса: {gameObject.name}");
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Игрок вышел из триггера ресурса");
            isPlayerInside = false;
        }
    }
}
