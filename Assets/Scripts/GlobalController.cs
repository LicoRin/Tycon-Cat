using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public int wood_amount = 0;
    public int stone_amount = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateResourceText(ResourceInfo res)
    {
        
        if (res.amountText != null)
        {
            res.amountText.text = $"{res.amount}";
            Debug.Log($"Обновлён текст для {res.name}: {res.amountText.text}");
        }
        else
        {
            Debug.LogWarning($"amountText не назначен для ресурса: {res.name}");
        }
    }
}
