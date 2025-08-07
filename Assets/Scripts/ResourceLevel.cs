using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;   
[System.Serializable]
public class LevelInfo
{
    public int level;
    public int amountPerCollect;
    public float collectTime;
    public float recoveryTime;
}

public class ResourceLevel : MonoBehaviour
{
    [Header("Level Settings")]
    public List<LevelInfo> levels = new List<LevelInfo>();
    public int currentLevel = 0;
    public string resourceName;

    
    public enum ResourceType { Wood, Stone, Money }
    public ResourceType resourceType;

    [Header("Resource References")]
    
    public InventoryItemInfo woodInfo;
    public InventoryItemInfo stoneInfo;
    public InventoryItemInfo moneyInfo;

    [Header("Animation Settings")]
    public Animator animator;

    private bool isPlayerInside = false;
    private bool isRecovering = false;
    private float collectProgress = 0f;

    private LevelInfo CurrentInfo => GetCurrentLevelInfo();

    private void Start()
    {
        SetAnimState(true, false, false);
    }
    public void UpdateMesh()
    {
        RuntimeNavMeshBaker.current.BakeNavMesh();
    }
    private void Update()
    {
        if (isPlayerInside && !isRecovering && CurrentInfo != null)
        {
            collectProgress += Time.deltaTime;
            SetAnimState(false, true, false);

            if (collectProgress >= CurrentInfo.collectTime)
            {
                CompleteCollection();
            }
        }
        else if (!isRecovering)
        {
            SetAnimState(true, false, false);
        }
    }

    private void CompleteCollection()
    {
        collectProgress = 0f;
        int amountToAdd = CurrentInfo.amountPerCollect;

        switch (resourceType)
        {
            case ResourceType.Wood:
                if (woodInfo != null)
                {
                    woodInfo.Initialize(
                        woodInfo.title,
                        woodInfo.description,
                        woodInfo.indentificator,
                        woodInfo.spriteIcon,
                        true,
                        woodInfo.amount + amountToAdd
                    );
                }
                break;

            case ResourceType.Stone:
                if (stoneInfo != null)
                {
                    stoneInfo.Initialize(
                        stoneInfo.title,
                        stoneInfo.description,
                        stoneInfo.indentificator,
                        stoneInfo.spriteIcon,
                        true,
                        stoneInfo.amount + amountToAdd
                    );
                }
                break;

            case ResourceType.Money:
                if (moneyInfo != null)
                {
                    moneyInfo.Initialize(
                        moneyInfo.title,
                        moneyInfo.description,
                        moneyInfo.indentificator,
                        moneyInfo.spriteIcon,
                        true,
                        moneyInfo.amount + amountToAdd
                    );
                }
                break;
        }

        GloballController.current?.UpdateResourceUI();

        StartCoroutine(StartRecovery(CurrentInfo.recoveryTime));
    }


    private IEnumerator StartRecovery(float recoveryTime)
    {
        isRecovering = true;
        collectProgress = 0f;
        SetAnimState(false, false, true);

        yield return new WaitForSeconds(recoveryTime);

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
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInside = false;
        }
    }
}
