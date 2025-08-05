using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class LevelInfo
{
    public int level;                // ������� �������
    public int amountPerCollect;     // ������� ��������� ��� �� ����
    public float collectTime;        // ����� ����� (���)
    public float recoveryTime;       // ����� �������������� (���)
}

public class ResourceLevel : MonoBehaviour
{
    [Header("Level Settings")]
    public List<LevelInfo> levels = new List<LevelInfo>();
    public int currentLevel = 0;

    [Header("Resource Settings")]
    public InventoryItemInfo inventoryItemInfo;  // ScriptableObject
    public Text amountText;                      // UI-����� ��� ����������� ����������

    private bool isCollecting = false;
    private bool isRecovering = false;

    // �������� ���������� � ������� ������
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
            Debug.Log($"����� ����� � ������� �������: {gameObject.name}");
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
            // ��������� ����� �����
            yield return new WaitForSeconds(info.collectTime);

            // ? ����������� ���������� � ScriptableObject
            inventoryItemInfo.Initialize(
                inventoryItemInfo.title,
                inventoryItemInfo.description,
                inventoryItemInfo.indentificator,
                inventoryItemInfo.spriteIcon,
                true,
                inventoryItemInfo.amount + info.amountPerCollect
            );

            Debug.Log($"{inventoryItemInfo.title} ������: {inventoryItemInfo.amount}");

            // ? ��������� UI
            if (amountText != null)
                amountText.text = inventoryItemInfo.amount.ToString();

            // ����� ����� ��������� ��������������
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
            Debug.Log("����� ����� �� �������� �������");
        }
    }
}
