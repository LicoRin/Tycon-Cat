using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public List<LevelInfo> levels = new List<LevelInfo>();
    public int currentLevel = 0;
    private bool isCollecting = false;
    private bool isRecovering = false;
     
    public ResourcesController res; // Ссылка на контроллер ресурсов



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
        else
        {
            Debug.Log($"В триггер ресурса {gameObject.name} вошёл объект с тегом: {collision.tag}");
        }
    }

    private IEnumerator StartCollecting()
    {
        isCollecting = true;
        LevelInfo info = GetCurrentLevelInfo();
        if (info != null)
        {
            yield return new WaitForSeconds(info.collectTime);
            res.CollectResource(gameObject.name, info.amountPerCollect);
           

            // Удалено: передача количества в ResourceData

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
