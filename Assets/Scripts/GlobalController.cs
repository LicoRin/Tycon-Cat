using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public int wood_amount = 0;
    public int stone_amount = 0;
    public ResourcesController resources;
    public static GlobalController Instance; // Глобальная ссылка

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Синхронизируем значения amount с resLev.totalAmount

       

    }

    void Awake()
    {
        // Если объект один — делаем его Instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Если на сцене вдруг окажется второй GlobalController
            Destroy(gameObject);
        }
    }


    public void UpdateResourceText(ResourceInfo res)
    {
        
        if (res.amountText != null)
        {
            res.amountText.text = $"{wood_amount}";
            Debug.Log($"Обновлён текст для {res.name}: {res.amountText.text}");
        }
        else
        {
            Debug.LogWarning($"amountText не назначен для ресурса: {res.name}");
        }
    }
}
