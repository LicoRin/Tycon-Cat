using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public int wood_amount = 0;
    public int stone_amount = 0;
    public ResourcesController resources;
    public static GlobalController Instance; // ���������� ������

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �������������� �������� amount � resLev.totalAmount

       

    }

    void Awake()
    {
        // ���� ������ ���� � ������ ��� Instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // ���� �� ����� ����� �������� ������ GlobalController
            Destroy(gameObject);
        }
    }


    public void UpdateResourceText(ResourceInfo res)
    {
        
        if (res.amountText != null)
        {
            res.amountText.text = $"{wood_amount}";
            Debug.Log($"������� ����� ��� {res.name}: {res.amountText.text}");
        }
        else
        {
            Debug.LogWarning($"amountText �� �������� ��� �������: {res.name}");
        }
    }
}
